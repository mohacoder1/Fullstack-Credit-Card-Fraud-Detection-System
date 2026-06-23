using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Database
{
    public static class clsDataAccessSettings
    {
      private static string _ConnectionString= "Server =.; Database=CreditCardFraudDetectionSystem;User Id = sa; Password=sa123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout = 30";
        
        public static string ConnectionString
        {
            get { return _ConnectionString; }
        }

    }
}
