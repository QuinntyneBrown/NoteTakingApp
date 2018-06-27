using MediatR;
using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Common
{
    public class VersionedCommandBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IMediator _mediator;
        private readonly IEntityVersionManager _entityVersionManager;
        
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
                    var inner = versionedRequest.InnerRequest;
                    var entityName = versionedRequest.EntityName;
                    var entityIdProperty = inner.GetType().GetProperty($"{versionedRequest.EntityName}Id");
                    var entityId = default(int);
                    var version = default(int);
                    
                    if (entityIdProperty != null)
                    {                                            
                        entityId = Convert.ToInt16(entityIdProperty.GetValue(inner));
                        version = Convert.ToInt16(inner.GetType().GetProperty($"Version").GetValue(inner));                        
                        entityVersion = _entityVersionManager.Acquire(entityId, entityName, version);                        
                        inner.GetType().GetProperty($"Version").SetValue(inner, entityVersion.Version);                        
                        return await _mediator.Send(inner);
                    }

                    var entity = inner.GetType().GetProperty(entityName).GetValue(inner);                    
                    entityId = Convert.ToInt16(entity.GetType().GetProperty($"{entityName}Id").GetValue(entity));

                    if (entityId == 0) return await _mediator.Send(inner);

                    version = Convert.ToInt16(entity.GetType().GetProperty($"Version").GetValue(entity));                    
                    entityVersion = _entityVersionManager.Acquire(entityId, entityName, version);
                    entity.GetType().GetProperty($"Version").SetValue(entity, entityVersion.Version);                                       
                    return await _mediator.Send(inner);   
                }
                catch
                {
                    if (entityVersion != default(EntityVersion))
                        await _entityVersionManager.Release(entityVersion);
                }
            }

            return await next();
        }
    }
}
