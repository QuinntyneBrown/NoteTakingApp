using MediatR;

namespace NoteTakingApp.Core.Common
{
    public class VersionedCommand
    {
        public interface IVersionedRequest<R>
        {
            IRequest<R> InnerRequest { get; }
            string Type { get; set; }
            string EntityName { get; set; }
        }

        public class Request<T, R> : IRequest<R>, IVersionedRequest<R>
            where T : IRequest<R>
        {
            public IRequest<R> InnerRequest { get; }
            public string Type { get; set; }
            public string EntityName { get; set; }
            public Request(T innerRequest, string type, string entityName)
            {
                EntityName = entityName;
                InnerRequest = innerRequest;
                Type = type;            }
        }        
    }
}
