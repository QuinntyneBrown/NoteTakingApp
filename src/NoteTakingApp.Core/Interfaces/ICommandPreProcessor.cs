using System;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Interfaces
{
    public interface IConcurrentCommandGuard
    {
        Task<TResponse> Process<TRequest, TResponse>(TRequest request, Func<TRequest,CancellationToken, Task<TResponse>> callback)
            where TRequest : ICommand<TResponse>;
    }
}
