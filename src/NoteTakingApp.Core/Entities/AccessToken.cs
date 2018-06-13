using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using NoteTakingApp.Core.Identity;

namespace NoteTakingApp.Core.Entities
{
    public class AccessToken: BaseEntity
    {
        public int AccessTokenId { get; set; }
        public string Value { get; set; }
        public DateTime ValidTo { get; set; }
        public string Username { get; set; }
        public static AccessToken Create(string value, string username, DateTime validTo) {            
            var accessToken = new AccessToken()
            {
                Value = value,
                Username = username,
                ValidTo = validTo
            };

            accessToken.RaiseDomainEvent(new AccessTokenCreatedDomainEvent(accessToken));

            return accessToken;
        }
    }
}
