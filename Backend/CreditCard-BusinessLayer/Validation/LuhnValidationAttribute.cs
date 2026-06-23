using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CreditCard_BusinessLayer.Validation
{
    public class LuhnValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success; // نترك التحقق من الفراغ لـ [Required]

            string cardNumber = value.ToString().Replace(" ", ""); // إزالة المسافات إن وجدت

            if (!cardNumber.All(char.IsDigit))
            {
                return new ValidationResult("رقم البطاقة يجب أن يحتوي على أرقام فقط.");
            }

            if (IsValidLuhn(cardNumber))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("رقم البطاقة غير صحيح (فشل في اختبار Luhn).");
        }

        private bool IsValidLuhn(string number)
        {
            int sum = 0;
            bool alternate = false;

            // نبدأ من اليمين إلى اليسار
            for (int i = number.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(number[i].ToString());

                if (alternate)
                {
                    n *= 2;
                    if (n > 9) n -= 9;
                }

                sum += n;
                alternate = !alternate;
            }

            return (sum % 10 == 0);
        }
    }
}
