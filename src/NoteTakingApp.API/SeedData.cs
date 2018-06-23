using NoteTakingApp.Core.Models;
using NoteTakingApp.Core.Extensions;
using NoteTakingApp.Core.Identity;
using NoteTakingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace NoteTakingApp.API
{
    public class SeedData
    {
        public static void Seed(AppDbContext context)
        {
            UserConfiguration.Seed(context);
            TagConfiguration.Seed(context);

            context.SaveChanges();
        }

        internal class UserConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                if (context.Users.IgnoreQueryFilters().FirstOrDefault(x => x.Username == "quinntynebrown@gmail.com") == null)
                {
                    var user = new User()
                    {
                        Username = "quinntynebrown@gmail.com"
                    };
                    user.Password = new PasswordHasher().HashPassword(user.Salt, "P@ssw0rd");

                    context.Users.Add(user);
                }

                if (context.Users.IgnoreQueryFilters().FirstOrDefault(x => x.Username == "kirkabrown@ymail.com") == null)
                {
                    var user = new User()
                    {
                        Username = "kirkabrown@ymail.com"
                    };
                    user.Password = new PasswordHasher().HashPassword(user.Salt, "P@ssw0rd");

                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
        }

        internal class TagConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                if (context.Tags.IgnoreQueryFilters().FirstOrDefault(x => x.Name == "Angular") == null)
                {
                    var tag = new Tag();
                    tag.Update("Angular");
                    context.Tags.Add(tag);
                }

                if (context.Tags.IgnoreQueryFilters().FirstOrDefault(x => x.Name == "ASP.NET Core") == null)
                {
                    var tag = new Tag();
                    tag.Update("ASP.NET Core");
                    context.Tags.Add(tag);
                }
                    

                context.SaveChanges();
            }
        }
    }
}
