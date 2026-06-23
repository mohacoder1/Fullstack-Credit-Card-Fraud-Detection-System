using CreditCard_DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_BusinessLayer
{
    public interface IAuthService
    {
        string GenerateToken(md_AuthPerson Person); // افترضنا أن عندك كلاس User
    }
}
