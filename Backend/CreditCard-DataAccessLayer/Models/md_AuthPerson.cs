using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Models
{
    public  class md_AuthPerson
    {
        public int ID { get; set; } 
        public string FullName { get; set; }
        public string PersonRole { get; set; }

        public int AccessLevel { get; set; }
        public bool isActive { get; set; }
        public string Email { get; set; }
        public string PhoneNumber{ get; set; }

        public md_AuthPerson(int ID, string FullName, string PersonRole,
            int AccessLevel, bool isActive , string Email,string PhoneNumber)
        {
            this.ID = ID;
            this.Email= Email; 
            this.PhoneNumber = PhoneNumber;
            this.AccessLevel = AccessLevel;

            this.isActive= isActive;
            this.FullName= FullName;
            this.PersonRole = PersonRole;
        }
        
    }
}
