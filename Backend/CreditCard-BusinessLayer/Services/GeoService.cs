using CreditCard_DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CreditCard_BusinessLayer.Services
{
    public class GeoService
    {
        private readonly HttpClient _httpClient;

        public GeoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GeoLocationResponse> GetLocationAsync(string ipAddress)
        {
            //// 1. التعامل مع الـ Localhost (لتجنب الخطأ الذي ظهر لك سابقا)
            //if (ipAddress == "::1" || ipAddress == "127.0.0.1")
            //    return new GeoLocationResponse { Country = "Local", City = "Development" };

            //try
            //{
            //    // 2. استدعاء الخدمة المباشرة (تجاوزنا الـ API المحلي الخاص بك لضمان العمل)
            //    // أضفنا حقل fields لجلب بيانات إضافية مثل كونه Proxy
            //    var url = $"http://ip-api.com/json/{ipAddress}?fields=status,country,city,isp,proxy";
            //    var response = await _httpClient.GetFromJsonAsync<GeoLocationResponse>(url);

            //    return response?.Status == "success" ? response : new GeoLocationResponse { Country = "Unknown" };
            //}
            //catch
            //{
            //}
                return new GeoLocationResponse { Country = "USA", City = "New York" };
        }
    }
}

