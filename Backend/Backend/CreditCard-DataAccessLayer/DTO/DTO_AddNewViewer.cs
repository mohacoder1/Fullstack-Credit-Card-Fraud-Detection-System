using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO
{
    public class DTO_AddNewViewer
    {
        public int? ViewerID { get; set; }
        public required string FirstName { get; set; }
        public required string SecondName { get; set; }
        public required string ThirdName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber
        {
            get; set;
        }
        public required string NationalID { get; set; }
        public required string PasswordHash { get; set; }
        public required string Country { get; set; }
        public required string DateOfBirth { get; set; }
        public int? AccessLevel { get; set; }
    }
}
