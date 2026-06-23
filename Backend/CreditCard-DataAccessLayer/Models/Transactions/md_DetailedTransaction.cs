using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Models.Transactions
{
    public class md_DetailedTransaction
    {
        public  int TransactionID {  get; set; }
        public  int CardholderID {  get; set; }
        public string Name {  get; set; }
        public string MaskedCardNumber {  get; set; }
        public string Amount {  get; set; }
        public string TransactionStatus {  get; set; }         
        public  int CardID {  get; set; }
        public string CardType {  get; set; }
        public string IssueCountry {  get; set; }
        public string MerchantCategory {  get; set; }
        public string Location {  get; set; }
        public string DeviceType{  get; set; }
        public DateTime ExpiryDate {  get; set; }
        public DateTime TransactionDate {  get; set; }
        public string IP_Address{  get; set; }
        public string RiskLevel{  get; set; }
        public string Reason{  get; set; }

       public md_DetailedTransaction(int TransactionID, string Name, string MaskedCardNumber, string Amount, string TransactionStatus, int CardID, string CardType,
            string IssueCountry, string MerchantCategory, string Location, string DeviceType, string IP_Address, string RiskLevel, string Reason,
            DateTime ExpiryDate, DateTime TransactionDate, int CardholderID)
        {
            this.TransactionID = TransactionID;
            this.Name= Name;
            //-------------------------//

            this.MaskedCardNumber = MaskedCardNumber;
            this.Amount = Amount;
            this.TransactionStatus = TransactionStatus;
            this.CardID = CardID;
            this.CardType= CardType;
            //-------------------------//
            this.IssueCountry = IssueCountry;
            this.MerchantCategory = MerchantCategory;
            this.Location = Location;
            this.DeviceType = DeviceType;
            this.IP_Address = IP_Address;
            //-------------------------//
            this.RiskLevel= RiskLevel;
            this.Reason = Reason;
            this.ExpiryDate= ExpiryDate;
            this.TransactionDate = TransactionDate;
            this.CardholderID = CardholderID;
        }
    }
}
