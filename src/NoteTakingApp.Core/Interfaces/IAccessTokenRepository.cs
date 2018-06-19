using NoteTakingApp.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Interfaces
{
    public interface IAccessTokenRepository
    {
        List<string> GetValidAccessTokenValues();

        Task<List<string>> GetValidAccessTokenValuesAsync();
        IQueryable<AccessToken> GetValidAccessTokens();
        List<AccessToken> GetValidTokensByUsername(string username);
        Task InvalidateByUsername(string username);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void Add(AccessToken accessToken);
    }
}
