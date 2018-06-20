using MediatR;
using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Features.Identity
{
    public class SignOutCommand
    {
        public class Request : IRequest<Response> {
            public string Username { get; set; }
        }

        public class Response { }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken) {
                var user = await _context.Users.SingleAsync(x => x.Username == request.Username);

                user.RaiseDomainEvent(new UserSignedOutEvent.DomainEvent(user));

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { };
            }
        }
    }
}
