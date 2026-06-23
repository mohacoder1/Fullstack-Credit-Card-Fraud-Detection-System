using CreditCard_BusinessLayer;
using CreditCard_BusinessLayer.Services;
using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using UtilLayer;

namespace CreditCardDetectionSystemApi.Controllers
{
    namespace CreditCardDetectionSystemApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly OtpService _otpService;
            private readonly PersonService _personService;
            private readonly AuthService _authService;

            // نحقن فقط الـ OtpService لأنه هو من يدير العملية الآن
            public AuthController(OtpService otpService, PersonService personService, AuthService authService)
            {
                _otpService = otpService;
                _personService = personService;
                _authService = authService;
            }

            [HttpPost("send-otp")]
            public async Task<IActionResult> SendOtp([FromBody] DTO_OtpRequest request)
            {
                // request.Channel قد تكون "Email" أو "WhatsApp" قادمة من الفرونت إند
                if (string.IsNullOrEmpty(request.Identifier) || string.IsNullOrEmpty(request.Channel)) return BadRequest("البيانات ناقصة.");

                try
                {
                    if (await _personService.verificationRequest(request))
                    {
                        return Ok(new { message = $"تم إرسال الرمز عبر {request.Channel} بنجاح." });
                    }
                    return BadRequest(new { message = "فشل الإرسال." });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "فشل الإرسال.", error = ex.Message });
                }
            }

            [HttpPost("verify-otp")]
            public IActionResult VerifyOtp([FromBody] VerifyOtpRequest request)
            {
                if (string.IsNullOrEmpty(request.Identifier) || string.IsNullOrEmpty(request.Code))
                    return BadRequest(new { message = "البيانات غير مكتملة." });

                if (_otpService.VerifyOtp(request.Identifier, request.Code))
                {
                    return Ok(new { message = "تم التحقق بنجاح!" });
                    // نصيحة: هنا يمكنك إنشاء الـ JWT Token وإرجاعه للمستخدم
                }

                return BadRequest(new { message = "الرمز غير صحيح أو انتهت صلاحيته." });
            }


            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] DTO_LoginRequest loginRequest)
            {
                // 1. التحقق من المدخلات (Validation)
                if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Identifier))
                    return BadRequest("الرجاء إدخال البيانات المطلوبة");

                // 2. استدعاء البروسيجر من قاعدة البيانات
                // ملاحظة: الـ Repository يجب أن يعيد كائن من نوع md_AuthPerson
                var person = await PeopleRepository.Login(loginRequest.Identifier, hashingPassword.HashPassword(loginRequest.Password));
                //var person = await PeopleRepository.Login(loginRequest.Identifier, loginRequest.Password);

                // 3. التحقق من النتيجة (بناءً على البروسيجر الخاص بك)
                if (person == null || !person.isActive)
                {
                    return Unauthorized(new { message = "بيانات الدخول غير صحيحة أو الحساب غير نشط" });
                }

                var token = _authService.GenerateToken(person);

                // 5. إرجاع النتيجة للمتصفح (أو تطبيق React)
                return Ok(new
                {
                    Token = token,
                    Id = person.ID,
                    Name = person.FullName,
                    Role = person.PersonRole,
                    person.Email,
                    person.AccessLevel,
                    person.isActive,
                    person.PhoneNumber
                    //ExpiresIn = "10 Hours" // أو القيمة التي حددتها في JwtOptions
                });
            }


            [HttpPatch("ResetPassword")]
            public async Task<IActionResult> ResetPassword([FromBody] DTO_ResetPasswordRequest resetRequest)
            {
                if (resetRequest == null || string.IsNullOrEmpty(resetRequest.Identifier) || string.IsNullOrEmpty(resetRequest.Password))
                    return BadRequest("الرجاء إدخال البيانات المطلوبة");
                var result = await _personService.ResetPassword(resetRequest);
                //var result = await PeopleRepository.ResetPassword(resetRequest.Identifier, resetRequest.NewPassword);
                if (result)
                {
                    return Ok(new { message = "تم تحديث كلمة المرور بنجاح" });
                }
                else
                {
                    return BadRequest(new { message = "فشل تحديث كلمة المرور. الرجاء التحقق من البيانات المدخلة." });
                }


            }
        }
    }
}