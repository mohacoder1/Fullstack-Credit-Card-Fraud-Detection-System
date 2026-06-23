using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.Repository;
using CreditCard_DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilLayer;

namespace CreditCard_BusinessLayer.Services
{
    public  class PersonService
    {
        private readonly OtpService _otpService;
        private readonly AuthService _authService;

        public PersonService(OtpService otpService, AuthService authService)
        {
            this._otpService = otpService; 
            this._authService = authService;
        }
        private async static Task<md_AuthPerson> GetProfileByIdentifier(string Identifier)
        {
            var person = await PeopleRepository.GetProfileByIdentifier(Identifier);
            if (person == null)
            {
                throw new Exception("Person not found");
            }
            return person;
        }
        public async Task<bool> verificationRequest(DTO_OtpRequest request)
        {
            var person = await GetProfileByIdentifier(request.Identifier);
            if (person.Email == request.Identifier)
            {
                await _otpService.GenerateAndSendOtpAsync(request.Identifier, request.Channel);
                return true;
            }

            return false;
        }
        private bool IsOtpValid(string identifier, string inputOtp)
        {
            return _otpService.VerifyOtp(identifier, inputOtp);
        }
        public async Task<bool> RequestLogin(DTO_LoginRequest loginRequest)
        {
            var person = await PeopleRepository.Login(loginRequest.Identifier, hashingPassword.HashPassword(loginRequest.Password));

            if (person != null)
                 return await verificationRequest(new DTO_OtpRequest { Identifier = loginRequest.Identifier, Channel = "Email" });

            return false;
        }
        public async Task<bool> ResetPassword(DTO_ResetPasswordRequest resetReq)
        {
          
            string passwordHash = hashingPassword.HashPassword(resetReq.Password);

            var user = await GetProfileByIdentifier(resetReq.Identifier);
            // 3. تحديث قاعدة البيانات فعلياً
            // ملاحظة: الـ Role هنا يجب أن يكون محدد (مثلاً "Cardholder")
            int status = await PeopleRepository.ResetPassword(user.ID, passwordHash);

            if (status == 1)
            {
                return true;
            }

            return false;
        }

    }
}
