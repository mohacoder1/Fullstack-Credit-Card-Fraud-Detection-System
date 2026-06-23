using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Models.Transactions
{
    public class md_Transactions
    {
        public int TransactionID { get; set; }
        public int CardholderID { get; set; }
        public string Name { get; set; }
        public string MaskedCardNumber { get; set; }
        public string Amount { get; set; }
        public DateTime Date { get; set; }
        public string RiskLevel { get; set; }
        public string TransactionType { get; set; }
        public string  Status{ get; set; }
        


        public md_Transactions(int TransactionID , int CardholderID, string Name , string RiskLevel, string  Amount,
            string TransactionType, string MaskedCardNumber, DateTime Date,string Status
            )
        {
            this.Name = Name;
            this.CardholderID = CardholderID;
            this.MaskedCardNumber=MaskedCardNumber;
            this.TransactionType = TransactionType;
            this.TransactionID = TransactionID;
            this.Date = Date;
            this.Amount = Amount;
            this.RiskLevel = RiskLevel;
            this.Status = Status;
        }

    }
}
