
using Azure;
using Azure.Core;
using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.DTO.Transactions;
using CreditCard_DataAccessLayer.Models;
using CreditCard_DataAccessLayer.Models.Transactions;
using CreditCard_DataAccessLayer.Repository;
using Microsoft.Extensions.Logging;
using ML_Model;
using Org.BouncyCastle.Asn1.Esf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Transactions;
using UAParser;

namespace CreditCard_BusinessLayer.Services
{

    public class TransactionsService
    {
        
        private readonly TransactionsRepository _repository;
        private readonly HttpClient httpClient;
        private readonly CardsService _service;
        private readonly GeoService _geoService;
        private readonly IEncryptionService encryptionService;

        public TransactionsService(HttpClient client, CardsService cardsService , GeoService _geoService, TransactionsRepository _repository , IEncryptionService encryptionService)
        {
            this.httpClient = client;
            this._service = cardsService;
            this._geoService=_geoService;
            this._repository=_repository;
            this.encryptionService = encryptionService;

        }
        public async Task<DTO_ExecuteResult> ProcessFullTransactionAsync(DTO_AnalyzeRequest request, string ip, string userAgent)
        {
            
            // 1. جمع المعلومات الاستخباراتية (Parallel - لسرعة الأداء)
            var locationTask = _geoService.GetLocationAsync("8.8.8.8");
            var DeviceType=GetDeviceType(userAgent);
            var cardTask = _service.GetCardByID(request.CardID);
            await Task.WhenAll(locationTask, cardTask);

            var location = locationTask.Result;
            var card = cardTask.Result;

            // 2. بناء طلب الموديل الذكي بمنطق حقيقي
            var mlRequest = new DTO_MlRequest
            {
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"), // ISO string
                TimeOfDay = _getTimeOfDay(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")),
                Amount = request.Amount,
                Country = location.Country,
                IsInternational = false,// (card.IssueCountry != location.Country), // منطق حقيقي!
                DeviceType =DeviceType,
                CardType = card.CardType,
                MerchantCategory = request.MerchantCategory,
            };

            // 3. الحصول على التقييم
            var prediction = await PredictAsync(mlRequest);
           

            // 4. تنفيذ البروسيجر النهائي
            var finalData = new DTO_AddNewTransaction
            {
                CardID = card.CardID,
                ExternalFraudScore = (prediction.prediction == "Fraud" ? prediction.Confidence : 1.0 - prediction.Confidence) * 100,
                Amount = request.Amount,
                Country = location.Country,
                Currency = request.Currency,
                DeviceType = DeviceType,
                IP_Address = ip,
                Location_City = location.City,
                MerchantCategory = request.MerchantCategory,
                TransactionType = request.TransactionType,
               
            };

            return await _repository.ExecuteTransaction(finalData); ;
            }

        public async Task<bool> FinalizeAfterOtpAsync(DTO_FinalizeAfterOtp request)
        {
            return await _repository.FinalizeAfterOtp(request);
        }
        public List<md_Transactions>GetAllMyTransactions( int CardolderID)
        {
            List<md_Transactions>myTransactions= _repository.GetAll().Where(Tr => Tr.CardholderID==CardolderID).ToList();
            myTransactions.ForEach(Tr =>
            {
                Tr.MaskedCardNumber = encryptionService.Decrypt(Tr.MaskedCardNumber);
                Tr.MaskedCardNumber = "**** **** **** " + Tr.MaskedCardNumber.Substring(Tr.MaskedCardNumber.Length - 4); // تحويل الوقت إلى التوقيت المحلي
            });
            
            return myTransactions;
        }
        public List<md_Transactions> GetAllTransactions()
        {
            var Transactions= _repository.GetAll();
            Transactions.ForEach(Tr =>
            {
                Tr.MaskedCardNumber = encryptionService.Decrypt(Tr.MaskedCardNumber);
                Tr.MaskedCardNumber = "**** **** **** " + Tr.MaskedCardNumber.Substring(Tr.MaskedCardNumber.Length - 4); // تحويل الوقت إلى التوقيت المحلي
            });

            return Transactions;
        }
        public md_DetailedTransaction GetDetaileTransaction(int ID)
        {
            var transaction = _repository.GetTransactionById(ID);
            if (transaction != null)
            {
                transaction.MaskedCardNumber = encryptionService.Decrypt(transaction.MaskedCardNumber);
                transaction.MaskedCardNumber = "**** **** **** " + transaction.MaskedCardNumber.Substring(transaction.MaskedCardNumber.Length - 4);
            }
            return transaction;
        }

        public List<DTO_LatestTransaction> GetLatestTransaction(int CardholderID)
        {
            var transaction = _repository.GetLatestTransactionByByCardholderID(CardholderID);
            return transaction;
        }
        private  string _getTimeOfDay(string timestamp)
        {
            if (DateTime.TryParse(timestamp, out DateTime dt))
            {
                var hour = dt.Hour;
                if (hour >= 5 && hour < 12)
                    return "Morning";
                else if (hour >= 12 && hour < 17)
                    return "Afternoon";
                else if (hour >= 17 && hour < 21)
                    return "Evening";
                else
                    return "Night";
            }
            return "Unknown";
        }
        private  async Task<DTO_MLPredictionResult> PredictAsync(DTO_MlRequest RequestPayload)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew(); // بدء التوقيت فوراً
            var json = System.Text.Json.JsonSerializer.Serialize(RequestPayload);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("predict", content);
            sw.Stop(); 

            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadFromJsonAsync<DTO_MLPredictionResult>();
                var MLResult = new DTO_MLPredictionResult
                {
                    prediction = resultJson.prediction,
                    Confidence = resultJson.Confidence,
                    LatencyMs = sw.Elapsed.TotalMilliseconds

                };
                return MLResult;
            }
            return
                (new DTO_MLPredictionResult
                {
                    prediction = "Error",
                    Confidence = 0.0f,
                    LatencyMs=0.0
                    
                });
        }

        private string GetDeviceType(string userAgent)
        {
            // 1. الحصول على نص الـ User-Agent من الـ Headers الخاص بالطلب

            if (string.IsNullOrEmpty(userAgent)) return "Unknown";

            // 2. استخدام المكتبة لفك الشفرة
            var uaParser = Parser.GetDefault();
            ClientInfo client = uaParser.Parse(userAgent);

            // 3. تحديد النوع بناءً على خصائص الجهاز
            // المكتبة تصنف الأجهزة إلى (Mobile, Tablet, Desktop)
            if (client.Device.IsSpider) return "Bot"; // إذا كان محرك بحث

            // منطق بسيط للتصنيف
            string deviceFamily = client.Device.Family.ToLower();

            if (deviceFamily.Contains("iphone") || deviceFamily.Contains("android"))
                return "Mobile";
            else if (deviceFamily.Contains("ipad") || deviceFamily.Contains("tablet"))
                return "Tablet";
            else
                return "Desktop";
        }
        public async Task<object> Checkhealth()
        {
            try
            {
                // إرسال الطلب لـ Flask
                var response = await httpClient.GetAsync("health");

                // قراءة المحتوى بشكل غير متزامن
                var contentString = await response.Content.ReadAsStringAsync();

                return new
                {
                    health = contentString,
                    healthStatus = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                // في حال كان سيرفر Flask متوقفاً
                return new { health = "Error: Flask server unreachable", healthStatus = 500 };
            }
        }
    }
}
