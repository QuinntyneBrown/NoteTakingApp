using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using NoteTakingApp.Core.Identity;

namespace NoteTakingApp.Core.Entities
{
    public class AccessToken: Entity
    {
        public int AccessTokenId { get; set; }
        public string Value { get; set; }
        public DateTime ValidTo { get; set; }
        public string Username { get; set; }
    }
}
