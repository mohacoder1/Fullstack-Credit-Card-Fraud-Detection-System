using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO
{
    public class DTO_DetailedView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
        public string NationalID { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
    }
}
