using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Models
{
    public class GeoLocationResponse
    {

        [JsonPropertyName("status")] public string Status { get; set; }
        [JsonPropertyName("country")] public string Country { get; set; }
        [JsonPropertyName("city")] public string City { get; set; }
        [JsonPropertyName("isp")] public string Isp { get; set; } // مزود الخدمة (مهم لكشف الاحتيال)
        [JsonPropertyName("proxy")] public bool IsProxy { get; set; } // هل هو بروكس
    }
}

