using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace CreditCard_BusinessLayer.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var emailSettings = _config.GetSection("EmailSettings");
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("FraudShield System", emailSettings["Username"]));
            message.To.Add(new MailboxAddress("User", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(emailSettings["Host"], int.Parse(emailSettings["Port"]), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(emailSettings["Username"], emailSettings["Password"]);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                // ملاحظة مهمة لمشروعك:
                // إذا فشل الإيميل، نريد فقط تسجيل الخطأ وليس إيقاف النظام بالكامل
                System.Diagnostics.Debug.WriteLine($"Email Error: {ex.Message}");
                // يمكنك هنا رمي Exception مخصص إذا كنت تريد إبلاغ المستخدم
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }


            // --- السيناريوهات الأربعة ---

            // 1. إرسال رمز التحقق (للدخول)
            public async Task SendOtpAsync(string email, string otp)
            {
                string body = $@"<div style='direction: rtl; font-family: Arial; text-align: center;'>
                                <h2>رمز التحقق من FraudShield</h2>
                                <h1 style='color: blue; letter-spacing: 5px;'>{otp}</h1>
                                <p>هذا الرمز صالح لمدة 5 دقائق.</p>
                             </div>";
                await SendEmailAsync(email, "رمز التحقق OTP", body);
            }

            // 2. إشعار تغيير الرمز
            public async Task SendPasswordChangedAsync(string email)
            {
                string body = "<div style='direction: rtl; text-align: center;'><h2 style='color: green;'>✅ تم تغيير كلمة المرور بنجاح</h2><p>إذا لم تكن أنت من قام بذلك، تواصل معنا فوراً.</p></div>";
                await SendEmailAsync(email, "تحديث أمني - تغيير كلمة المرور", body);
            }

            // 3. عملية مشبوهة (Fraud Alert)
            public async Task SendFraudAlertAsync(string email, string last4Digits, decimal amount)
            {
                string body = $@"<div style='direction: rtl; text-align: center; border: 2px solid red; padding: 20px;'>
                                <h2 style='color: red;'>⚠️ تنبيه: محاولة احتيال محتملة!</h2>
                                <p>تم رصد محاولة دفع بمبلغ <b>{amount}$</b> على البطاقة المنتهية بـ <b>{last4Digits}</b>.</p>
                                <p>لقد قمنا بتجميد البطاقة مؤقتاً لحمايتك.</p>
                             </div>";
                await SendEmailAsync(email, "🚨 تنبيه أمني عاجل", body);
            }

            // 4. رفع الحجب
            public async Task SendUnblockAlertAsync(string email, string last4Digits)
            {
                string body = $@"<div style='direction: rtl; text-align: center;'><h2 style='color: blue;'>🔓 تم رفع الحجب</h2><p>تم إعادة تنشيط بطاقتك المنتهية بـ <b>{last4Digits}</b> بنجاح.</p></div>";
                await SendEmailAsync(email, "تنشيط البطاقة", body);
            }
    }
}
