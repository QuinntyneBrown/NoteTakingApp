using NoteTakingApp.Core.Interfaces;
using System.Security.Cryptography;

namespace NoteTakingApp.Core.Models
{
    public class User: Entity, IAggregateRoot
    {
        public User()
        {
            Salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(Salt);
            }
        }

        public int UserId { get; set; }        
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; private set; }
    }
}
