using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CreditCard_DataAccessLayer.DTO
{
    public sealed class DTO_FraudAlert
    {
        public int TransactionID { get; set; }
        public string AlertLevel { get; set; }
        public string Alertstatus { get; set; }
        public string CreatedByUsername { get; set; }
    }
}
