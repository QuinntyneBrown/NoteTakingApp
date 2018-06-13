using NoteTakingApp.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Interfaces
{
    public interface IAccessTokenRepository
    {
        List<string> GetValidAccessTokenValues();
        List<AccessToken> GetByUsername(string username);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
