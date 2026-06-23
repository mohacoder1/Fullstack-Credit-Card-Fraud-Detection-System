using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CreditCard_BusinessLayer.Services
{
    public class AccessControlService
    {
        public enum enUserRole { Cardholder = 1, Viewer = 2 }

        public enum enAccessLevel
        {
            Cardholder = 3,
            Viewer = 5,   // المحلل (الموظف العادي)
            Admin = 10     // السوبر أدمن (المدير)
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccessControlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public int GetUserId()
        {
            var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out int result) ? result : 0;
        }

        //public enUserRole? GetUserRole()=>enUserRole.Viewer;
        //public enAccessLevel GetAccessLevel() => enAccessLevel.Admin; // أعلى صلاحية
        public enUserRole? GetUserRole()
        {
            var roleClaim = User?.FindFirst(ClaimTypes.Role)?.Value;
            return Enum.TryParse(roleClaim, out enUserRole role) ? role : null;
        }

        public enAccessLevel GetAccessLevel()
        {
            var levelClaim = User?.FindFirst("AccessLevel")?.Value;
            return Enum.TryParse(levelClaim, out enAccessLevel level) ? level : enAccessLevel.Cardholder;
        }

        public bool IsAdmin() => GetAccessLevel() == enAccessLevel.Admin;
        public bool IsViewer() => GetAccessLevel() == enAccessLevel.Viewer;
    }
}

