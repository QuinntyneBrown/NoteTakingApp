using NoteTakingApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<EntityVersion> EntityVersions { get; set; }
        DbSet<Note> Notes { get; set; }
        DbSet<AccessToken> AccessTokens { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
