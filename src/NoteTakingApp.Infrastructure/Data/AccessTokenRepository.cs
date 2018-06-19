using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Entities;
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
        public AccessTokenRepository(IAppDbContext context)
            => _context = context;

        public void Add(AccessToken accessToken)
            => _context.AccessTokens.Add(accessToken);

        public List<AccessToken> GetValidTokensByUsername(string username)
            => GetValidAccessTokens().Where(x => x.Username == username).ToList();

        public List<string> GetValidAccessTokenValues()
            => GetValidAccessTokens().Select(x => x.Value).ToList();

        public Task<List<string>> GetValidAccessTokenValuesAsync() {
            return GetValidAccessTokens().Select(x => x.Value).ToListAsync();
        }

        public IQueryable<AccessToken> GetValidAccessTokens()
            => _context.AccessTokens.Where(x => x.ValidTo > DateTime.UtcNow);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
            => _context.SaveChangesAsync(cancellationToken);
            
        public async Task InvalidateByUsername(string username)
        {
            foreach (var accessToken in GetValidTokensByUsername(username))
                accessToken.ValidTo = default(DateTime);

            await SaveChangesAsync(default(CancellationToken));            
        }
    }
}
