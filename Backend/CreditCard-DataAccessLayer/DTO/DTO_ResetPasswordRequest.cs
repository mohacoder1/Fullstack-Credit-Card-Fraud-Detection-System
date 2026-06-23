using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO
{
    public class DTO_ResetPasswordRequest
    {
        public string  Identifier { get; set; }
        public string Password { get; set; }
    }
}
