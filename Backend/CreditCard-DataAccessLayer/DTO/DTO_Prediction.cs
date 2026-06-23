using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CreditCard_DataAccessLayer.DTO
{
    public sealed class DTO_Prediction
    {
        public int TransactionID { get; set; }
      
        public double ConfidenceScore { get; set; } = 0;
        public double FraudScore { get; set; } = 0;

    }
}
