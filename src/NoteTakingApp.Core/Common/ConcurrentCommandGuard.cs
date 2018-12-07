using AsyncUtilities;
using NoteTakingApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Common
{
    public class ConcurrentCommandGuard : IConcurrentCommandGuard
    {
        private readonly ICommandRegistry _registry;

        private readonly AsyncLock _lock = new AsyncLock();

        public ConcurrentCommandGuard(ICommandRegistry registry) => _registry = registry;

        public async Task<TResponse> Process<TRequest, TResponse>(TRequest request, Func<TRequest,CancellationToken, Task<TResponse>> asyncCallback)
            where TRequest : ICommand<TResponse>
        {
            var tcs = new TaskCompletionSource<TResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
            var dependentKeys = default(IEnumerable<string>);
            var partition = default(string);

            try
            {
                using (await _lock.LockAsync())
                    dependentKeys = await _registry.Register(partition, request.Key, request.SideEffects);

                if (dependentKeys.Count() > 0)
                {
                    _registry.Subscribe(async (commandRegisteryChanged) =>
                    {
                        if (dependentKeys.Contains($"{commandRegisteryChanged.Partition}-{commandRegisteryChanged.Key}") && !_registry.ContainsAny(dependentKeys).GetAwaiter().GetResult())
                            tcs.SetResult(await asyncCallback(request,default(CancellationToken)));
                    });

                    return await tcs.Task;
                }
                else
                    return await asyncCallback(request, default(CancellationToken));
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                using (await _lock.LockAsync())
                    await _registry.Clean(partition, request.Key);
            }
        }
    }
}
