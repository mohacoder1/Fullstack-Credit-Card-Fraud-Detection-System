

using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.Models;
using CreditCard_DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UtilLayer;
namespace CreditCard_BusinessLayer.Services
{
    public class CardholderService
    {
        const string Role = "Cardholder";
        public DTO_AddNewCardholder CardHolder { get; set; }
        private readonly IEncryptionService EncryptionService;
        private readonly OtpService _otpService;

        public CardholderService(DTO_AddNewCardholder AddNew , IEncryptionService service, OtpService otpService) 
        {
            this._otpService = otpService;
        CardHolder = AddNew;
        EncryptionService = service;
        }

        public DTO_DetailedCardholder GetDetailedCardholder(int CardholderID) 
        {
            var cardholder = CardholdersRepository.GetCardholderInDetails(CardholderID).Result;
            if (cardholder != null)
            {
                cardholder.Email = MaskingEmail(cardholder.Email);
            }
            return cardholder;
        }
        public async Task<bool>AddNew()
        {


            CardHolder.PasswordHash= hashingPassword.HashPassword(CardHolder.PasswordHash);
            CardHolder.CVV= EncryptionService.Encrypt(CardHolder.CVV);
            CardHolder.CardNumber= EncryptionService.Encrypt(CardHolder.CardNumber);


            var ID = await CardholdersRepository.CreateCardholder(CardHolder);
            return ID>0;
        }
        public static async Task<bool> isCardholderActive(int cardholderID)
        {
            return await PeopleRepository.isPersonActive(cardholderID , Role);
        }
        public static async Task<bool> setAccountStatus(int cardholderID, bool status)
        {
            return await PeopleRepository.SetAccoutStatus(cardholderID, status , Role);
        }
        public static async Task<md_Cardholders?> GetCardholderByEmail(string Email)
        {
            return await CardholdersRepository.GetCardholderByEmail(Email);
        }
        public static async Task<md_Cardholders?> GetCardHolderByID(int cardholderID)
        {
            return await CardholdersRepository.GetCardholderByID(cardholderID);
        }
        public static async Task<List<md_Cardholders?>> GetAll()
        {
           return await CardholdersRepository.GetAllCardholders();
        }
        public static async Task<string>ChangePassword(int CardholderID,string OldPassword,string NewPassword)
        {
            NewPassword=hashingPassword.HashPassword(NewPassword);
            OldPassword=hashingPassword.HashPassword(OldPassword);
            int status= await PeopleRepository.ChangePassword(CardholderID, OldPassword, NewPassword , Role);


            switch (status)
            {
                case 1: return "Succss";
                case -2: return "Wrong old Password";
                case -1: return "ID Not Exists";
                default: return "Something wrong";
            }

        }
       
        public static async Task<bool> DeleteCardholder(int cardholderID)
        {
            return await setAccountStatus(cardholderID, false);
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
