using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO.Dashboard
{
    public class DTO_FraudTrendItem
    {
        public int Month { get; set; }
        public int FraudCount { get; set; }
        public int LegitCount { get; set; }
    }
}
