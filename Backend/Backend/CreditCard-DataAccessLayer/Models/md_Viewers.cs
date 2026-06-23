using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Models
{
    public class md_Viewers
    {

        public int ViewerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalID { get; set; }
        public string isActive { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int AccessLLevel { get; set; }


        public md_Viewers(int ViewerID, string Name, string Email, string PhoneNumber, string NationalID, string isActive,
            string Country, DateTime DateOfBirth, int AccessLevel)
        {
            this.AccessLLevel = AccessLevel;
            this.Name = Name;
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
            this.NationalID = NationalID;
            this.isActive = isActive;
            this.Country = Country;
            this.DateOfBirth = DateOfBirth;
            this.ViewerID = ViewerID;
        }
    }
}
