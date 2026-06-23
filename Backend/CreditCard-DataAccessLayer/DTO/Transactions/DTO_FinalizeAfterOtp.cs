using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO.Transactions
{
    public class DTO_FinalizeAfterOtp
    {
        public int CardID { get; set; }
        public int transactionId { get; set; }
        public double amount { get; set; }
    }
}
