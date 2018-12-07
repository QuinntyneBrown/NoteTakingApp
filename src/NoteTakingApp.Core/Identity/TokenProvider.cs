using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoteTakingApp.Core.Identity
{
    public interface ITokenManager
    {
        string Issue(string username);
        DateTime GetValidToDateTime(string token);
    }

    public class TokenManager : ITokenManager
    {
        private IConfiguration _configuration;
        public TokenManager(IConfiguration configuration)
            => _configuration = configuration;
        
        public string Issue(string uniqueName)
        {
            var now = DateTime.UtcNow;
            var nowDateTimeOffset = new DateTimeOffset(now);

            var claims = new List<Claim>()
                {
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", uniqueName),
                    new Claim(JwtRegisteredClaimNames.UniqueName, uniqueName),
                    new Claim(JwtRegisteredClaimNames.Sub, uniqueName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, nowDateTimeOffset.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                };
            
            var jwt = new JwtSecurityToken(
                issuer: _configuration["Authentication:JwtIssuer"],
                audience: _configuration["Authentication:JwtAudience"],
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(Convert.ToInt16(_configuration["Authentication:ExpirationMinutes"])),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:JwtKey"])), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public DateTime GetValidToDateTime(string token) {
            return (new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken).ValidTo;
        }
    }
}
