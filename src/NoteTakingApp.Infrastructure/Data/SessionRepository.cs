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
    public class SessionRepository : ISessionRepository
    {
        private readonly IAppDbContext _context;
        private readonly IDistributedCache _distributedCache;
        
        public SessionRepository(IAppDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        public void Add(Session session)
            => _context.Sessions.Add(session);

        public Task<List<Session>> GetByUsernameAsync(string username)
            => GetConnectedSessions().Where(x => x.Username == username).ToListAsync();
        
        public async Task<List<string>> GetValidAccessTokenValuesAsync()
            => await _distributedCache
            .GetOrAdd(() => GetConnectedSessions().Select(x => x.AccessToken).ToListAsync(), "ActiveSessions");

        public async Task<IQueryable<Session>> GetConnectedSessionsAsync()
            => await Task.FromResult(GetConnectedSessions());

        public IQueryable<Session> GetConnectedSessions() => _context.Sessions.Where(x => x.SessionStatus == SessionStatus.Connected);

        public IQueryable<User> GetActiveUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _distributedCache.RemoveAsync("ActiveSessions", cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }
            
        public async Task InvalidateByUsernameAsync(string username)
        {
            foreach (var session in await GetByUsernameAsync(username))
                session.SessionStatus= SessionStatus.Invalid;           
        }
    }
}
