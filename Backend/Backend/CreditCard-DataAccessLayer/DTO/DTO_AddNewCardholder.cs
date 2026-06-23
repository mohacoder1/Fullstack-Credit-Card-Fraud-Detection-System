using CreditCard_BusinessLayer.Validation;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO
{
    public sealed class DTO_AddNewCardholder
    {

        public required string FirstName { get; set; }
        public required string SecondName { get; set; }
        public  required string ThirdName { get; set; }
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

        //[Required(ErrorMessage = "رقم البطاقة مطلوب")]
        //[StringLength(16, MinimumLength = 13, ErrorMessage = "طول الرقم غير منطقي")]
        //[LuhnValidation]
        public string  CardNumber { get; set; }
        public string  CardType { get; set; }
        public double Balance { get; set; }
        public string CVV { get; set; }

    }
}
