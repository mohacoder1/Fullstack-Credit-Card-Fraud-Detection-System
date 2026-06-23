using CreditCard_BusinessLayer.Services;
using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreditCardDetectionSystemApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly CardsService cardsService;
        private readonly AccessControlService _accessControl;

        public CardsController(CardsService service, AccessControlService accessControl)
        {
            cardsService = service;
            _accessControl = accessControl;
        }


        //[Authorize(Roles = "Viewer")]
        [HttpGet("GetCardsInfo")]
        public async Task<ActionResult<IEnumerable<md_Cards>>> GetCardInfo()
        {
            var cardInfo = await cardsService.GetCardsInfo();
            if (cardInfo == null)
            {
                return NotFound("Not Found");
            }
            return Ok(cardInfo);
        }

        //[Authorize(Roles = "Cardholder")]
        [HttpGet("GetMyCardsInfo")]
        public async Task<ActionResult<IEnumerable<md_Cards>>> GetMyCardInfo(int ID)
        {
            var cardInfo = await cardsService.GetMyCardsInfo(ID);
            if (cardInfo == null)
            {
                return NotFound("Not Found");
            }
            return Ok(cardInfo);
        }

        [HttpGet("GetCardInfoByID")]
        public async Task<ActionResult<md_Cards>> GetCardInfoByID(int CardID)
        {
            var cardInfo = await cardsService.GetCardByID(CardID);
            if (cardInfo == null)
            {
                return NotFound("Not Found");
            }
            return Ok(cardInfo);
        }

       

       

        //[Authorize(Roles = "Viewer")]
        [HttpGet("CheckBalance")]
        public async Task<ActionResult<bool>> CheckBalance(int CardID, double WithdrawAmount)
        {
            bool result=await cardsService.CheckBalance(CardID, WithdrawAmount);
            return Ok(result);
        }

        //[Authorize(Roles = "Cardholder")]
        [HttpPost("AddNew")]
        public async Task<ActionResult<bool>> AddNew([FromBody]DTO_AddNewCard newCard)
        {
            if (!ModelState.IsValid)
            {
                // إذا فشل اختبار Luhn، سيرجع الخطأ هنا تلقائياً
                return BadRequest(ModelState);
            }
            bool result = await cardsService.AddNew(newCard);
            return Ok(result);
        }

        //[Authorize(Roles = "Viewer")]
        [HttpPatch("UpdateCardStatus")]
        public async Task<ActionResult<bool>> UpdateCardStatus([FromBody]DTO_UpdateCardStatus DTO)
        {
            var isSuccess = await cardsService.UpdateCardStatus(DTO);
            if (isSuccess)
            {
                return Ok(true);
            }
            return false;
        }

        //[Authorize(Roles = "Viewer")]
        [HttpPatch("UpdateCardBalance")]
        public async Task<ActionResult<bool>> UpdateCardBalance(int ID, double Amount)
        {
            var isSuccess = await cardsService.UpdateCardBalance(ID, Amount);
            if (isSuccess)
            {
                return Ok(true);
            }
            return false;
        }

   
    }
}
