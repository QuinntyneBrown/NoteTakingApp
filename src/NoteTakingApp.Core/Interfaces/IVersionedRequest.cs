using MediatR;

namespace NoteTakingApp.Core.Interfaces
{
    public interface IVersionedRequest<R> : IRequest<R>
    {
        IRequest<R> InnerRequest { get; }
        string EntityName { get; set; }
    }
}
