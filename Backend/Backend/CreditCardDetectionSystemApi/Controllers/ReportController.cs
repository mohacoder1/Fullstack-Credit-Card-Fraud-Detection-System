using CreditCard_BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreditCardDetectionSystemApi.Controllers
{
    //[Authorize(Roles = "Viewer")]
    [Route("api/Report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        //private readonly AccessControlService _service;

        //public ReportController(AccessControlService service) 
        //{
        //    _service = service;
        //}

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await DashboardRepository.GetDashboardChartsAsync();
            return Ok(result);
        }
        [HttpGet("getSummary")]
        public IActionResult GetSummaryReport()
        {
            try
            {
                //if (!_service.IsAnalyst() && !_service.IsAdmin())
                //{
                //    return Forbid("هذه التقارير مخصصة لطاقم العمل فقط");
                //}

                var data = ReportServices.GetSummeryReport();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("getSummaryByID")]
        public IActionResult GetSummaryReportByID(int ID)
        {
            try
            {
                //if (!_service.IsAnalyst() && !_service.IsAdmin())
                //{
                //    return Forbid("هذه التقارير مخصصة لطاقم العمل فقط");
                //}

                var data = ReportServices.GetSummeryReportByID(ID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
