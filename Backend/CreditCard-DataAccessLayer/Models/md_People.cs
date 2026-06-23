using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Models
{
    public class md_People
    {

        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalID { get; set; }
        public string PasswordHash { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool isActive { get; set; }

        public md_People(int PersonID , string FirstName, string SecondName,
            string ThirdName, string LastName, string Email, string PhoneNumber,
            string NationalID, string PasswordHash, string Country, DateTime DateOfBirth, bool isActive)
        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.ThirdName = ThirdName;
            this.SecondName = SecondName;
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
            this.NationalID = NationalID;
            this.PasswordHash = PasswordHash;
            this.Country = Country;
            this.DateOfBirth = DateOfBirth;
            this.isActive = isActive;
        }
    }
}
