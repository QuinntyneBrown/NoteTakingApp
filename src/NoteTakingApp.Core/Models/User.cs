using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace NoteTakingApp.Core.Entities
{
    public class User: Entity
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
        [Required]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public byte[] Salt { get; private set; }
    }
}
