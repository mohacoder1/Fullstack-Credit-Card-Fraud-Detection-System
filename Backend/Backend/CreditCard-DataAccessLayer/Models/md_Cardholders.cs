using CreditCard_DataAccessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Models
{
    public class md_Cardholders
    {
        public int CardholderID {  get; set; }
        public string NationalID {  get; set; }
        public string Name {  get; set; }
        public string Email {  get; set; }
        public string PhoneNumber {  get; set; }
        public string Country {  get; set; }
        public DateTime JoinDate {  get; set; }
        public string MembershipLevel {  get; set; }
        public int? CardsCount {  get; set; }
        public string isActive {  get; set; }



        public md_Cardholders(int CardholderID,string NationalID , string Name, string Email, string PhoneNumber,
            string Country,DateTime JoinDate, string MembershipLevel ,int CardsCount ,string isActive)
        {
            this.CardholderID = CardholderID;
            this.Name = Name;
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
            this.NationalID = NationalID;
            this.isActive = isActive;
            this.Country = Country;
            this.JoinDate = JoinDate;
            this.MembershipLevel = MembershipLevel;
            this.CardsCount = CardsCount;
        }
    }
}
