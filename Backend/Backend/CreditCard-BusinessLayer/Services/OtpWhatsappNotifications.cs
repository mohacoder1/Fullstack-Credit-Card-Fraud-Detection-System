using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CreditCard_BusinessLayer.Services
{
    public class OtpWhatsappNotifications
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public OtpWhatsappNotifications(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        // 1. الدالة الأساسية أصبحت تستقبل الرقم (toPhone) كباراميتر
        private async Task SendWhatsAppAsync(string toPhone, string message)
        {
            var settings = _config.GetSection("WhatsAppSettings");
            var instId = settings["InstanceId"];
            var token = settings["Token"];

            // تنظيف الرقم من أي مسافات أو علامة +
            string cleanNumber = toPhone.Replace("+", "").Trim();

            var url = $"https://api.ultramsg.com/{instId}/messages/chat";

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("token", token),
                new KeyValuePair<string, string>("to", cleanNumber), // نستخدم الرقم القادم من المستخدم
                new KeyValuePair<string, string>("body", message)
            });

            try
            {
                var response = await _httpClient.PostAsync(url, data);
                var result = await response.Content.ReadAsStringAsync();

                // --- السطر السحري الذي سيكشف لنا المشكلة ---
                if (result.Contains("error") || result.Contains("false") || result.Contains("invalid"))
                {
                    // هذا سيوقف البرنامج ويرمي الخطأ لكي تراه في Swagger
                    throw new Exception($"رفض من UltraMsg: {result}");
                }
            }
            catch (Exception ex)
            {
                // رمي الخطأ للـ Controller بدلاً من إخفائه
                throw new Exception(ex.Message);
            }
        }

        // 2. دالة إرسال الـ OTP (نمرر لها الـ Identifier)
        public async Task SendOtpAsync(string identifier, string otp)
        {
            string whatsappMsg = $"🛡️ *FraudShield System*\n\n" +
                                 $"عزيزي المستخدم، رمز التحقق الخاص بك هو:\n" +
                                 $"*[{otp}]*\n\n" +
                                 $"⚠️ لا تشارك هذا الرمز مع أي شخص.";

            // نمرر الـ identifier (الذي هو رقم الهاتف) إلى محرك الإرسال
            await SendWhatsAppAsync(identifier, whatsappMsg);
        }

        // 3. دالة التنبيه عن الاحتيال
        public async Task SendFraudAlertAsync(string identifier, string amount, string last4Digits)
        {
            string message = $"🚨 *تنبيه أمني: FraudShield*\n\n" +
                             $"تم رصد عملية دفع مشبوهة بقيمة *{amount}$*\n" +
                             $"على البطاقة المنتهية بـ *({last4Digits})*.\n\n" +
                             $"إذا لم تكن أنت من قام بهذه العملية، يرجى تجميد البطاقة فوراً من التطبيق.";

            await SendWhatsAppAsync(identifier, message);
        }
    }
}