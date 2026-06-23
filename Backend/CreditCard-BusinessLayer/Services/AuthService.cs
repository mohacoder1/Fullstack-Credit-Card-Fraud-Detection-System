using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.Extensions.Options;
using CreditCard_DataAccessLayer.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_BusinessLayer.Services
{
    public class AuthService: IAuthService
    {
        private readonly md_JwtOptions _jwtOptions;

        public AuthService(IOptions<md_JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(md_AuthPerson person)
        {
            var authClaims = new List<Claim>
            {
        // المعرف الفريد للمستخدم
        new Claim(ClaimTypes.NameIdentifier, person.ID.ToString()),
        
        // الاسم الكامل والإيميل
        new Claim(ClaimTypes.Name, person.FullName),
        new Claim(ClaimTypes.Email, person.Email),
        
        // الدور الوظيفي (Cardholder أو Viewer)
        new Claim(ClaimTypes.Role, person.PersonRole),
        
        // معلومات إضافية مفيدة لنظامك
        new Claim("PhoneNumber", person.PhoneNumber ?? ""),
        new Claim("AccessLevel", person.AccessLevel.ToString()),
        new Claim("IsActive", person.isActive.ToString().ToLower()),
        
        // معرف فريد للتوكن لمنع هجمات التكرار (Replay Attacks)
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                expires: DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
