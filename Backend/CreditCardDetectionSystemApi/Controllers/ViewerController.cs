using CreditCard_BusinessLayer.Services;
using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.Models;
using CreditCard_DataAccessLayer.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CreditCardDetectionSystemApi.Controllers
{
    [Authorize(Roles = "Viewer")]
    [Route("api/Viewer")]
    [ApiController]
    public class ViewerController : ControllerBase
    {
        private readonly AccessControlService _accessControl;
        public ViewerController(AccessControlService accessControlService)
        {
            _accessControl = accessControlService;
        }

        [HttpGet("GetDetailedViewer")]
        public async Task<ActionResult<DTO_DetailedView>> GetDetailedViewer(int ID)
        {
            try
            {
                //if (!_accessControl.IsAdmin() && _accessControl.GetUserId() != ID)
                //{
                //    return Forbid("لا يمكنك رؤية تفاصيل زميلك إلا إذا كنت مديراً");
                //}
                var viewerDetails = await ViewersService.GetViewerDetailsByID(ID);
                if (viewerDetails != null)
                {
                    return Ok(viewerDetails);
                }
                return NotFound("المستخدم غير موجود");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("SetAcountStatus")]
        public async Task<ActionResult<bool>> setAccountStatus(int ID, bool Status)
        {

            //if (!_accessControl.IsAdmin())
            //    return Forbid("تعديل حالات الحسابات من صلاحيات المدير فقط");

            var isSuccess = await ViewersService.setAccountStatus(ID, Status);
            if (isSuccess)
            {
                return Ok(true);
            }
            return false;
        }
        [HttpGet("GetAllViewers")]
        public async Task<ActionResult<IEnumerable<md_Viewers>>> GetAllUsers()
        {
            //if (!_accessControl.IsAdmin())
            //    return Forbid("تعديل حالات الحسابات من صلاحيات المدير فقط");

            var users = await ViewersService.GetAll();
            return Ok(users);
        }

        [HttpGet("GetViewerByID")]
        public async Task<ActionResult<bool>> GetViewerByID(int ID)
        {
            try
            {
                //if (!_accessControl.IsAdmin())
                //    return Forbid("تعديل حالات الحسابات من صلاحيات المدير فقط");

                var Viewer =  await ViewersService.GetViewerByID(ID);
               
                return Ok(Viewer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("DeleteViewer")]
        public async Task<ActionResult<bool>> DeleteUser(int ID)
        {
            {
                try
                {
                    //if (!_accessControl.IsAdmin())
                    //    return Forbid("تعديل حالات الحسابات من صلاحيات المدير فقط");

                    bool isDeleted =  await ViewersService.Delete(ID);
                   return Ok(isDeleted);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }

        [HttpPost("AddViewer")]
        public async Task<ActionResult<string>> AddNew([FromBody] DTO_AddNewViewer viewer)
        {

            try
            {
                //    if (!_accessControl.IsAdmin())
                //        return Forbid("تعديل حالات الحسابات من صلاحيات المدير فقط");


                var Viewer = new ViewersService(viewer);
                bool isSuccess = await Viewer.AddNew();
                return isSuccess ? Ok(isSuccess) : NotFound(false);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPatch("ChangePassword")]
        public async Task<ActionResult<string>> ChangePassword(int ID, string OldPassword, string NewPassword)
        {
            //var currentUserId = _accessControl.GetUserId();

            //// القاعدة: يمكنك تغيير باسووردك بنفسك، أو المدير يغير لأي أحد
            //if (currentUserId != ID && !_accessControl.IsAdmin())
            //{
            //    return Forbid("لا يمكنك تغيير كلمة مرور زميلك إلا إذا كنت مديراً");
            //}
            var Message = await ViewersService.ChangePassword(ID, OldPassword, NewPassword);
           
               return Ok(Message);
            
        }

        [HttpGet("isActive")]
        public async Task<ActionResult<bool>> isActive(int ID)
        {

            //if (!_accessControl.IsAdmin())
            //{
            //    return Forbid("لا تملك الصلاحيات لهذه الحركة ");
            //}

            
            var isActive = await ViewersService.isUserActive(ID);
            if (isActive)
            {
                return Ok(true);
            }
            return false;
        }
    }
}