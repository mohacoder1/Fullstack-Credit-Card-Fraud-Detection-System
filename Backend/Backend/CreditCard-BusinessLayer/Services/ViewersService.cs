using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.Models;
using CreditCard_DataAccessLayer.Repository;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using UtilLayer;

namespace CreditCard_BusinessLayer.Services
{
    public class ViewersService
    {
        const string Role = "Viewer";
        
        public DTO_AddNewViewer Viewer { get; set; }
        public ViewersService(DTO_AddNewViewer AddNew)
        {
            Viewer = AddNew;
        }
        public static async Task<DTO_DetailedView?> GetViewerDetailsByID(int ViewerID)
        {
            var viewer = await ViewersRepository.GetViewerInDetails(ViewerID);
            if (viewer != null)
            {
                viewer.Email = MaskingEmail(viewer.Email);
            }
            return viewer;
        }
        public async Task<bool> AddNew()
        {
            Viewer.PasswordHash = hashingPassword.HashPassword(Viewer.PasswordHash);
            Viewer.ViewerID = await ViewersRepository.CreateViewer(Viewer);

            return Viewer.ViewerID > 0;
        }
        public static async Task<md_Viewers?> GetViewerByID(int ViewerID)
        {
            return await ViewersRepository.GetViewerByID(ViewerID);
        }
        public static async Task<List<md_Viewers>> GetAll()
        {
            return await ViewersRepository.GetAll();
        }
     
        public static async Task<bool> isUserActive(int ViewerID)
        {
            return await PeopleRepository.isPersonActive(ViewerID, Role);
        }
        public static async Task<bool> setAccountStatus(int ViewerID, bool status)
        {
            return await PeopleRepository.SetAccoutStatus(ViewerID, status , Role);
        }
        public static async Task<string> ChangePassword(int ViewerID, string OldPassword, string NewPassword)
        {
            NewPassword = hashingPassword.HashPassword(NewPassword);
            OldPassword = hashingPassword.HashPassword(OldPassword);
            int status = await PeopleRepository.ChangePassword(ViewerID, OldPassword, NewPassword , Role);
            switch (status)
            {
                case 1: return "Succss";
                case -2: return "Wrong old Password";
                case -1: return "ID Not Exists";
                default: return "Something wrong";
            }
        }
       
        public static async Task<bool> Delete(int ViewerID)
        {
            return await setAccountStatus(ViewerID, false);
        }
        private static string MaskingEmail(string Email)
        {
            //mohammed@gmail.com ->m******d@gmail.com
            if (!string.IsNullOrEmpty(Email))
            {
                int atIndex = Email.IndexOf('@');
                if (atIndex > 1)
                {
                    string maskedEmail = Email[0] + new string('*', atIndex - 2) + Email.Substring(atIndex - 1);
                    Email = maskedEmail;
                }
            }
            return Email;
        }
    }
}