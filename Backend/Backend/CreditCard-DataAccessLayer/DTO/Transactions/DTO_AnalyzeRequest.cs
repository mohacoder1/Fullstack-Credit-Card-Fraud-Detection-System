using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CreditCard_DataAccessLayer.DTO.Transactions
{
    public class DTO_AnalyzeRequest
    {
        public int CardholderID { get; set; }
        public int CardID { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string MerchantCategory {  get; set; }
        public string TransactionType {  get; set; }

    }
}
