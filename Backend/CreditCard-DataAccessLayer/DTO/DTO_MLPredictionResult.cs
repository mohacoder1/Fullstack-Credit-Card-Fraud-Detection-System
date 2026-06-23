using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO
{
    public class DTO_MLPredictionResult
    {
        public string prediction { get; set; }
        public double Confidence { get; set; }
        public double LatencyMs { get; set; } // أضف هذا الحقل
    }
}
