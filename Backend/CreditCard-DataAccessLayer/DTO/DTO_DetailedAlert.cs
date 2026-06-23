using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO
{
    public  class DTO_DetailedAlert
    {
        public int AlertID { get; set; }
        public string AlertLevel { get; set; }
        public string AlertStatus { get; set; }
        public DateTime AlertTime { get; set; }
        public double RiskScore { get; set; }

        //تفاصيل الحركة
        public int TX_Number { get; set; }
        public string Currency { get; set; }
        public string MerchantCategory { get; set; }
        public string TransactionReason { get; set; }
        public string TransactionType { get; set; }
        public double Amount { get; set; }

        //3. معلومات حامل البطاقة (العميل)

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ResidentCountry { get; set; }

        //4-معلومات البطاقة والحساب
        public string CardNumber { get; set; }
        public string CardStatus { get; set; }
        public double CurrentBalance { get; set; }

        public string SystemRecommendation { get; set; }
    }
}
