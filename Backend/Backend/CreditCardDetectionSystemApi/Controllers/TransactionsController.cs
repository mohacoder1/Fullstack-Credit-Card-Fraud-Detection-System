using CreditCard_BusinessLayer.Services;
using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.DTO.Transactions;
using CreditCard_DataAccessLayer.Models;
using CreditCard_DataAccessLayer.Models.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.VisualBasic;
using ML_Model;
using System.Diagnostics;
using System.Text;
using UAParser;


namespace CreditCardDetectionSystemApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionsService transactionsService;
        //private readonly AccessControlService _accessControl; // الخدمة الجديدة
        public TransactionsController(TransactionsService transactionsService)// , AccessControlService service)
        {
             this.transactionsService = transactionsService;
          // _accessControl = service;

        }
        [HttpGet("Health")]
        public async Task<IActionResult> Health()
        {
            var res = await transactionsService.Checkhealth();
            return Ok(res);
        }

        //[Authorize(Roles = "Viewer")]
        [HttpGet("GetAllTransactions")]
        public ActionResult<IEnumerable<md_Transactions>> GetAllTransactions()
        {
            try
            {
                var transactions = transactionsService.GetAllTransactions();
                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound("There arnt any transactioithe system ");
                }
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetLatestTransaction")]
        public ActionResult<List<DTO_LatestTransaction>> GetLatestTransaction(int cardholderId)
        {
            try
            {
                var transaction = transactionsService.GetLatestTransaction(cardholderId);
                if (transaction == null)
                {
                    return NotFound("There aren't any transactions in the system ");
                }
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        //[Authorize(Roles = "Cardholder")]
        [HttpGet("GetAllMyTransactions")]
        public ActionResult<IEnumerable<md_Transactions>> GetAllMyTransactions(int ID)
        {
            try
            {
                
                var transactions = transactionsService.GetAllMyTransactions(ID);
                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound("There arnt any transactioithe system ");
                }
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[Authorize] 
        [HttpGet("GetDetailTransaction")]
        public ActionResult<md_DetailedTransaction> GetDetailTransaction(int ID)
        {
            try
            {
                var transaction =  transactionsService.GetDetaileTransaction(ID);
                if (transaction== null)
                {
                    return NotFound("There arn\'t any transaction in the system ");
                }
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[Authorize(Roles = "Cardholder")]
        [HttpPost("ExecuteTransaction")]
        public async Task<ActionResult<DTO_ExecuteResult>> ExecuteTransaction([FromBody] DTO_AnalyzeRequest request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await transactionsService.ProcessFullTransactionAsync(request, ip, Request.Headers["User-Agent"]);

            // التعديل: إذا قاعدة البيانات قالت Flagged (بسبب الـ 120$) 
            // أو الموديل قال Fraudulent وسكوره مرتفع، في كلا الحالتين اطلب OTP
            if (result.Status == "Flagged" || result.Status == "Fraudulent" )
            {
                result.Status = "REQUIRES_OTP";
            }

            return Ok(result);
        }
        [HttpPost("FinalizeAfterOtp")]
        public async Task<ActionResult<bool>> FinalizeAfterOtp([FromBody] DTO_FinalizeAfterOtp otp)
        {
            var result = await transactionsService.FinalizeAfterOtpAsync(otp);

            return Ok(result);
        }

    }
}