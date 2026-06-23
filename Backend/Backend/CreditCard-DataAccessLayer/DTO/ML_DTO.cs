using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO
{
    public sealed class ML_DTO
    {
        public float Amount { get; set; }
        public string Country { get; set; } = "";
        public string TransactionType { get; set; } = "";
        public string CardType { get; set; } = "";
        public string MerchantCategory { get; set; } = "";
        public string DeviceType { get; set; } = "";
        public bool IsInternational { get; set; }
    }
}
