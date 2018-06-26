using NoteTakingApp.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Interfaces
{
    public interface IEntityVersionRepository
    {
        EntityVersion Get(int entityId, string entityName);
        void Create(EntityVersion entityVersion);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        void SaveChanges();
        Task Remove(EntityVersion entityVersion);
    }
}
