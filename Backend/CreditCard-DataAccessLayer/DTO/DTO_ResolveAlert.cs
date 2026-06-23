using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO
{
    public class DTO_ResolveAlert
    {
        public int AlertID { get; set; }
        public int viewerID { get; set; }
        public bool ResolveStatus { get; set; }
        public string Action {  get; set; }
        public string Comment {  get; set; }
    }
}
