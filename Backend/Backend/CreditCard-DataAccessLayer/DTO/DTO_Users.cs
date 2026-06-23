using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO
{
    public sealed class DTO_Users
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public int PermissionsMask { get; set; }
        public bool isActive { get; set; }
        public string Email { get; set; } = string.Empty;

        public bool isAdmin { get { return PermissionsMask == 2; } }
    }
}
