

using System.ComponentModel.DataAnnotations;
using CreditCard_BusinessLayer.Validation;

namespace CreditCard_DataAccessLayer.DTO
{
    public sealed class DTO_AddNewCard

    {

        public int CardholderID { get; set; }

        //[Required(ErrorMessage = "رقم البطاقة مطلوب")]
        //[StringLength(16, MinimumLength = 13, ErrorMessage = "طول الرقم غير منطقي")]
        //[LuhnValidation]
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string CardType { get; set; }
        public double Balance { get; set; }
       
    }
}
