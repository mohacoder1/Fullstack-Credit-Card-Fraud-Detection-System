using CreditCard_DataAccessLayer.Models;
using CreditCard_BusinessLayer.Services;
using CreditCard_BusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CreditCard_DataAccessLayer.DTO;
using Microsoft.AspNetCore.Authorization;

namespace CreditCardDetectionSystemApi.Controllers
{
    [AllowAnonymous]
    [Route("api/Cardholders")]
    [ApiController]

    public class CardholdersController : ControllerBase
    {
        private readonly AccessControlService _accessControl;
        private readonly IEncryptionService encryptionService;
        private readonly OtpService _otpService;

        public CardholdersController(AccessControlService accessControl, IEncryptionService service, OtpService otpService)
        {
            _accessControl = accessControl;
            encryptionService = service;
            _otpService = otpService;
        }

        
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<md_Cardholders>>>GetAll()
        {

            if (!_accessControl.IsViewer() && !_accessControl.IsAdmin())
                return Forbid("ليس لديك الصلاحيات للوصول لهذه الحركة");

            var CardHolders = await CardholderService.GetAll();
            return Ok(CardHolders);
        }
        
        [Authorize (Roles = "Viewer")]
        [HttpGet("GetByID")]
        public async Task<ActionResult<md_Cardholders>> GetByID(int ID)
        {
            var CardHolder = await CardholderService.GetCardHolderByID(ID);

            return Ok(CardHolder);
        }
     
        [Authorize (Roles = "Viewer")]
        [HttpGet("GetByEmail")]
        public async Task<ActionResult<md_Cardholders>> GetByEmail(string Email)
        {
            var CardHolder = await CardholderService.GetCardholderByEmail(Email);

            return Ok(CardHolder);
        }

        [Authorize (Roles = "Viewer")]
        [HttpPatch("SetAcountStatus")]
        public async Task<ActionResult<bool>> setAccountStatus(int ID, bool Status)
        {
            var isSuccess = await CardholderService.setAccountStatus(ID, Status);
            if (isSuccess)
            {
                return Ok(true);
            }
            return false;
        }

       
        [Authorize(Roles = "Cardholder")]
        [HttpPatch("ChangePassword")]
        public async Task<ActionResult<string>> ChangePassword(int ID, string OldPassword, string NewPassword)
        {
            var CurrentID = _accessControl.GetUserId();
            if (CurrentID != ID && !_accessControl.IsAdmin())
            {
                return Forbid("لا يمكنك سوى تغيير كلمه السر خاصتك ");
            }
            var Message = await CardholderService.ChangePassword(ID, OldPassword, NewPassword);
           
                return Ok(Message);
          
        }

        [Authorize(Roles = "Viewer")]
        [HttpGet("isActive")]
        public async Task<ActionResult<bool>> isActive(int ID)
        {
            var isActive = await CardholderService.isCardholderActive(ID);
            if (isActive)
            {
                return Ok(true);
            }
            return false;
        }

        [HttpGet("GetDetailedCardholder")]
        public async Task<ActionResult<DTO_DetailedCardholder>> GetDetailedCardholder(int ID)
        {
            var cardholder = new CardholderService(null, encryptionService, _otpService).GetDetailedCardholder(ID);
            if (cardholder != null)
            {
                return Ok(cardholder);
            }
            return NotFound();
        }

        [HttpPost("AddNew")]
        public async Task<ActionResult<bool>> AddNew([FromBody] DTO_AddNewCardholder dto)
        {
            //if (!ModelState.IsValid)
            //{
            //    // إذا فشل اختبار Luhn، سيرجع الخطأ هنا تلقائياً
            //    return BadRequest(ModelState);
            //}

            CardholderService cardholder = new CardholderService(dto,encryptionService, _otpService);
            bool isSuccess = await cardholder.AddNew();
            if (isSuccess)
            {
                return Ok(true);
            }
            return false;
        }

        [Authorize (Roles = "Viewer")]
        [HttpDelete("Delete")]
        public async Task<ActionResult<bool>> Delete(int ID)
        {
            var isSuccess = await CardholderService.DeleteCardholder(ID);
            if (isSuccess)
            {
                return Ok(true);
            }
            return false;
        }
    }
}
