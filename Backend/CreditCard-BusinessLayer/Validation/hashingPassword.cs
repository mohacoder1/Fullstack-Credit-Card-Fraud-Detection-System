using CreditCard_BusinessLayer.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UtilLayer
{
    public class hashingPassword
    {
        
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        public static bool VerifyPassword(string enteredPassword, string storedHash) 
        { 
            var enteredHash = HashPassword(enteredPassword); return enteredHash == storedHash; 
        }
    }
}
