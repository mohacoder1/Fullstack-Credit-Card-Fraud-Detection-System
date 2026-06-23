using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace CreditCard_BusinessLayer.Services
{
    public  class OtpService
    {
        private readonly IMemoryCache _cache;
        private readonly EmailService _emailService;
        private readonly OtpWhatsappNotifications _whatsappService; // إضافة الواتساب هنا

        public OtpService(IMemoryCache cache, EmailService emailService, OtpWhatsappNotifications whatsappService)
        {
            _cache = cache;
            _emailService = emailService;
            _whatsappService = whatsappService;
        }

        public async Task GenerateAndSendOtpAsync(string userIdentifier, string channel)
        {
            string otpCode = new Random().Next(100000, 999999).ToString();
            _cache.Set(userIdentifier, otpCode, TimeSpan.FromMinutes(5));

            if (channel.ToLower() == "whatsapp")
            {
                await _whatsappService.SendOtpAsync(userIdentifier, otpCode);
            }
            else // Default is Email
            {
                await _emailService.SendOtpAsync(userIdentifier, otpCode);
            }
        }

        // دالة التحقق من الرمز المدخل
        public bool VerifyOtp(string identifier, string inputOtp)
            {
                // محاولة جلب الرمز المخزن لهذا المعرف
                if (_cache.TryGetValue(identifier, out string savedOtp))
                {
                    if (savedOtp == inputOtp)
                    {
                        // إذا كان صحيحاً، نمسحه من الذاكرة كي لا يُستخدم مرة أخرى
                        _cache.Remove(identifier);
                        return true;
                    }
                }
                return false;
            }
        }
    }