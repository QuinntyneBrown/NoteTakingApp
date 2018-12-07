using NoteTakingApp.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Interfaces
{
    public interface ISessionRepository
    {
        Task<List<string>> GetValidAccessTokenValuesAsync();
        Task<IQueryable<Session>> GetConnectedSessionsAsync();
        Task InvalidateByUsernameAsync(string username);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void Add(Session accessToken);
    }
}
