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
        {
            _context = context;
        }

        public List<AccessToken> GetByUsername(string username)
        {
            return _context.AccessTokens.Where(x => x.Username == username && x.ValidTo > DateTime.UtcNow).ToList();
        }

        public List<string> GetValidAccessTokenValues()
        {
            return _context.AccessTokens.Where(x => x.ValidTo > DateTime.UtcNow).Select(x => x.Value).ToList();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken) {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
