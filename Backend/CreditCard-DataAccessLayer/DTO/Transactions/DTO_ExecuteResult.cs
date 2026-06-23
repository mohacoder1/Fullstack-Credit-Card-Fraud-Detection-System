using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO.Transactions
{
    public  class DTO_ExecuteResult
    {
        public int TransactionID { get; set; }
        public int AlertID { get; set; }
        public string Status { get; set; }
        public double Score { get; set; }
        public int Result {  get; set; }
        public string Reason {  get; set; }

    }
}
