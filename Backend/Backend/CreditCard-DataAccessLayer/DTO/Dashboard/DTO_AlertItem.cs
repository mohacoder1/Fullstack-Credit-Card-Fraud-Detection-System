using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO.Dashboard
{
    public class DTO_AlertItem
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public string Severity { get; set; } 
        public DateTime Timestamp { get; set; } 
    }
}
