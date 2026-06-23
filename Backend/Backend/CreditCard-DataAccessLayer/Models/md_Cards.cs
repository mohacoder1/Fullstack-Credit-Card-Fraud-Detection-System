
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CreditCard_DataAccessLayer.Models
{

    public class md_Cards
    {
        public int CardID { get; set; }
        public int CardholderID { get; set; }
        public string CardholderName { get; set; }
        public string DisplayNumber { get; set; }
        public string CardStatus { get; set; }
        public string CardType { get; set; }
        public string IssueCountry { get; set; }
        public string BaseCurrency { get; set; }
        public double Balance { get; set; }
        //public double RemainingDailyLimit { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool isExpire { get { return ExpiryDate < DateTime.Now; }  }


        public md_Cards(int cardID,string BaseCurrency,string IssueCountry, 
            int cardholderID, string displayNumber,  string cardStatus, 
            string cardType, double balance, 
            DateTime expiryDate, string CardholderName)
        {
            CardID = cardID;
            CardholderID = cardholderID;
            this.CardholderName = CardholderName;
            DisplayNumber = displayNumber;
            CardStatus = cardStatus;
            CardType = cardType;
            Balance = balance;
         
            ExpiryDate = expiryDate;
            this.BaseCurrency=BaseCurrency;
            this.IssueCountry=IssueCountry;
        }
    }
}