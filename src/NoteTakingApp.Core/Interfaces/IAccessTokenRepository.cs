using NoteTakingApp.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Interfaces
{
    public interface IAccessTokenRepository
    {
        Task<List<string>> GetValidAccessTokenValuesAsync();
        IQueryable<AccessToken> GetValidAccessTokens();
        Task<List<AccessToken>> GetValidTokensByUsernameAsync(string username);
        Task InvalidateByUsernameAsync(string username);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void Add(AccessToken accessToken);
    }
}
