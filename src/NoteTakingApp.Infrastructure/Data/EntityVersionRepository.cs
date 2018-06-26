using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NoteTakingApp.Core.Extensions;
using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Infrastructure.Data
{
    public class EntityVersionRepository: IEntityVersionRepository
    {
        private readonly IAppDbContext _context;
        private readonly IDistributedCache _distributedCache;
        private static readonly string _cacheKey = "EntityVersions";

        public EntityVersionRepository(IAppDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        public EntityVersion Get(int entityId, string entityName) 
            => _distributedCache.GetOrAddSync(() => _context.EntityVersions.ToList(), _cacheKey)
                .Where(x => x.EntityName == entityName && x.EntityId == entityId)
                .OrderByDescending(x => x.Version)
                .FirstOrDefault();

        public void Create(EntityVersion entityVersion)
            => _context.EntityVersions.Add(entityVersion);

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _distributedCache.RemoveAsync(_cacheKey, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void SaveChanges()
        {
            _distributedCache.Remove(_cacheKey);
            _context.SaveChangesAsync(default(CancellationToken)).GetAwaiter().GetResult();
        }

        public Task Remove(EntityVersion entityVersion)
        {
            _context.EntityVersions.Remove(entityVersion);
            return Task.CompletedTask;
        }
    }
}
