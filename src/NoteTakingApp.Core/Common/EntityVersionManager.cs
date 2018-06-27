using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Common
{
    public class EntityVersionManager: IEntityVersionManager
    {
        private readonly IEntityVersionRepository _entityVersionRepository;

        public EntityVersionManager(IEntityVersionRepository entityVersionRepository)
            => _entityVersionRepository = entityVersionRepository;

        public EntityVersion Acquire(int entityId, string entityName, int version)
        {
            var entityVersion = _entityVersionRepository.Get(entityId, entityName);

            if (entityVersion != null && version < entityVersion.Version)
                throw new DomainException("Older version!");
            
            var newEntityVersion = new EntityVersion()
            {
                EntityName = entityName,
                EntityId = entityId,
                Version = entityVersion == null ? version + 1 : entityVersion.Version + 1
            };

            _entityVersionRepository.Create(newEntityVersion);
            _entityVersionRepository.SaveChanges();
            return newEntityVersion;
        }
        
        public async Task Release(EntityVersion entityVersion)
        {
            await _entityVersionRepository.Remove(entityVersion);
            await SaveChangesAsync(default(CancellationToken));
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
            => await _entityVersionRepository.SaveChangesAsync(cancellationToken);
    }
}
