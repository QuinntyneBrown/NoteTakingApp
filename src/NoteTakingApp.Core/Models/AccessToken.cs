using NoteTakingApp.Core.Common;
using System;

namespace NoteTakingApp.Core.Models
{
    public class AccessToken
    {
        public int AccessTokenId { get; set; }
        public string Value { get; set; }
        public DateTime ValidTo { get; set; }
        public string Username { get; set; }
    }
}
