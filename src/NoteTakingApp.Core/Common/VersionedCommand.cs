using MediatR;
using NoteTakingApp.Core.Interfaces;

namespace NoteTakingApp.Core.Common
{
    public class VersionedCommand
    {
        public class Request<T, R> : IVersionedRequest<R>
            where T : IRequest<R>
        {
            public IRequest<R> InnerRequest { get; }            
            public string EntityName { get; set; }
            public Request(T innerRequest, string entityName)
            {
                EntityName = entityName;
                InnerRequest = innerRequest;                
            }
        }        
    }
}
