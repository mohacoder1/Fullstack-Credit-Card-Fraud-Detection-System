using CreditCard_BusinessLayer.Services;
using CreditCard_BusinessLayer;
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
    public class FraudAlertController : ControllerBase
    {
        private readonly IEncryptionService Service;

        public FraudAlertController(IEncryptionService service)
        {
            //this.FraudAlertService = fraudAlertService;
            this.Service = service;
        }

        [Authorize(Roles ="Viewer")]
        [HttpPatch("setAlertAcknowledge")]

        public ActionResult<bool> setAlertAcknowledge(int AlertID,int viewerID)
        {
            // Logic to close the fraud alert with the given alertId
            bool isSuccess = FraudAlertService.setAlertAcknowledge(AlertID,viewerID);
            if (isSuccess)
            {
                return Ok(new { Message = "Fraud alert closed successfully." });
            }
            else
            {
                return NotFound(new { Message = "Fraud alert not found." });
            }
        }

        [HttpPatch("ResolveAlert")]
        public ActionResult<bool> ResolveAlert([FromBody]DTO_ResolveAlert alert)
        {
            // Logic to close the fraud alert with the given alertId
            bool isSuccess = FraudAlertService.ResolveFraudAlert (alert);
            if (isSuccess)
            {
                return Ok(new { Message = "Fraud alert closed successfully." });
            }
            else
            {
                return NotFound(new { Message = "Fraud alert not found." });
            }
        }

        [HttpGet("GetDetailedAlert")]
        public ActionResult<DTO_DetailedAlert> GetDetailedAlert(int ID)
        {
            var fraudAlerts = FraudAlertService.GetDetailedAlert(ID , Service);
            return Ok(fraudAlerts);
        }
        [HttpGet("GetAll")]
        public ActionResult<List<md_FraudAlert>> GetAll()
        {
            var fraudAlerts = FraudAlertService.GetAllFraudAlerts();
            return Ok(fraudAlerts);
        }

        [HttpGet("GetAlertsByCardholderID")]
        public ActionResult<List<md_FraudAlert>> GetAlertsByCardholderID(int CardholderID)
        {
            var fraudAlerts = FraudAlertService.GetAlertsByCardholderID(CardholderID);
            return Ok(fraudAlerts);
        }

    }
}
