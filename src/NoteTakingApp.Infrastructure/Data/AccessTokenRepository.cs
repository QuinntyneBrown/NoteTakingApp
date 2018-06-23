using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NoteTakingApp.Core.Models;
using NoteTakingApp.Core.Extensions;
using NoteTakingApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Infrastructure.Data
{
    public class AccessTokenRepository : IAccessTokenRepository
    {
        private readonly IAppDbContext _context;
        private readonly IDistributedCache _distributedCache;
        
        public AccessTokenRepository(IAppDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        public void Add(AccessToken accessToken)
            => _context.AccessTokens.Add(accessToken);

        public Task<List<AccessToken>> GetValidTokensByUsernameAsync(string username)
            => GetValidAccessTokens().Where(x => x.Username == username).ToListAsync();

        public List<string> GetValidAccessTokenValues()
            => GetValidAccessTokens().Select(x => x.Value).ToList();

        public async Task<List<string>> GetValidAccessTokenValuesAsync()
            => await _distributedCache
            .GetOrAdd(() => GetValidAccessTokens().Select(x => x.Value).ToListAsync(), "ValidAccessTokens");

        public IQueryable<AccessToken> GetValidAccessTokens()
            => _context.AccessTokens.Where(x => x.IsValid);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _distributedCache.RemoveAsync("ValidAccessTokens", cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
        }
            
        public async Task InvalidateByUsernameAsync(string username)
        {
            foreach (var accessToken in await GetValidTokensByUsernameAsync(username))
                accessToken.IsValid= false;           
        }
    }
}
