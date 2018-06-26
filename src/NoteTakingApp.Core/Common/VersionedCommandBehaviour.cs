using MediatR;
using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using static NoteTakingApp.Core.Common.VersionedCommand;

namespace NoteTakingApp.Core.Common
{
    public class VersionedCommandBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IMediator _mediator;
        private readonly IEntityVersionManager _entityVersionManager;
        private object _lockObject = new object();

        public VersionedCommandBehavior(IMediator mediator, IEntityVersionManager entityVersionManager)
        {
            _entityVersionManager = entityVersionManager;
            _mediator = mediator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is IVersionedRequest<TResponse> versionedRequest)
            {
                var entityVersion = default(EntityVersion);
                try
                {
                    var inner = default(IRequest<TResponse>);
                    var entityId = default(int);
                    var version = default(int);

                    if (versionedRequest.Type == "Delete")
                    {
                        inner = versionedRequest.InnerRequest;                    
                        entityId = Convert.ToInt16(inner.GetType().GetProperty($"{versionedRequest.EntityName}Id").GetValue(inner));
                        version = Convert.ToInt16(inner.GetType().GetProperty($"Version").GetValue(inner));
                        entityVersion = _entityVersionManager.Acquire(entityId, versionedRequest.EntityName, version);
                        inner.GetType().GetProperty($"Version").SetValue(inner, entityVersion.Version);
                        await _entityVersionManager.SaveChangesAsync(cancellationToken);
                        return await _mediator.Send((request as IVersionedRequest<TResponse>).InnerRequest);
                    }


                    inner = versionedRequest.InnerRequest;
                    var entity = inner.GetType().GetProperty(versionedRequest.EntityName).GetValue(inner, null);
                    entityId = Convert.ToInt16(entity.GetType().GetProperty($"{versionedRequest.EntityName}Id").GetValue(entity, null));
                    version = Convert.ToInt16(entity.GetType().GetProperty($"Version").GetValue(entity));

                    if (entityId != 0)
                    {
                        entityVersion = _entityVersionManager.Acquire(entityId, versionedRequest.EntityName, version);
                        entity.GetType().GetProperty($"Version").SetValue(entity, entityVersion.Version);
                        await _entityVersionManager.SaveChangesAsync(cancellationToken);
                    }

                    return await _mediator.Send((request as IVersionedRequest<TResponse>).InnerRequest);
   
                }catch (Exception e)
                {
                    if (entityVersion != default(EntityVersion))
                        await _entityVersionManager.Release(entityVersion);
                }
            }

            return await next();
        }
    }
}
