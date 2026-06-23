using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO
{
    public class DTO_UpdateCardStatus
    {
        public string Status { get; set; }
        public int CardID { get; set; }
        public int viewerID { get; set; }

    }
}
