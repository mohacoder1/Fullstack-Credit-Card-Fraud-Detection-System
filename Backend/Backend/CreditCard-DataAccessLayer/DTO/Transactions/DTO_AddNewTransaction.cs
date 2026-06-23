using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CreditCard_DataAccessLayer.DTO.Transactions
{
    public class DTO_AddNewTransaction
    {

        public int CardID { get; set; }
        public double Amount { get; set; }
        public double ExternalFraudScore { get; set; }
        public string Currency { get; set; }=string.Empty;
        public string Country { get; set; }= string.Empty;
       
        public string DeviceType { get; set; } = string.Empty;
        public string MerchantCategory { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public string Location_City { get; set; }  = string.Empty;
        public string IP_Address { get; set; } = string.Empty;

    }
}
