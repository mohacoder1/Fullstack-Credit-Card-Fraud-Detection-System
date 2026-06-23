using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Models
{
    public class md_User
    {
        /*
         @Username varchar(50),
@Password varchar(150),
@PermissionsMask int, --Admin=2 , User=1
@isActive bit,
@UserID int output
         */
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int PermissionsMask { get; set; }
        public bool isActive { get; set; }
        public string Email { get; set; } = string.Empty;

        public bool isAdmin { get { return PermissionsMask == 2; } }

        public md_User(int userId , string Username , string Password  , int permissions , bool isActive)
        {
            UserID = userId;
            this.Username = Username;
            this.Password = Password;
            this.PermissionsMask = permissions;
            this.isActive = isActive;

        }

    }
}
