using CreditCard_BusinessLayer.Services;
using CreditCard_DataAccessLayer;
using CreditCard_DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace CreditCardDetectionSystemApi.Controllers
{
    [Authorize(Roles ="Viewer")]
    [Route("api/Logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        //private readonly AccessControlService _accessControl; // ال

        //public LogsController(AccessControlService accessControlService) 
        //{
        //_accessControl = accessControlService;
        //}

        [HttpGet("GetAll")]
        public   ActionResult<List<md_Logs>> GetAllLogs()
        {
            //if (!_accessControl.IsAdmin())
            //    return Forbid("لا تملك الصلاحيات لمثل هذه الخطوة");


            var logs =   LogsService.GetAllLogs();
            if (logs == null || logs.Count == 0)
            {
                return NotFound("No logs found.");
            }
            return Ok(logs);
        }

    }
}
