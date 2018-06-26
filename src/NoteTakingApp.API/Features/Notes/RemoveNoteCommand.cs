using FluentValidation;
using MediatR;
using NoteTakingApp.Core.DomainEvents;
using NoteTakingApp.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Features.Notes
{
    public class RemoveNoteCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.NoteId).NotEqual(default(int));
            }
        }
        public class Request : IRequest<Response>
        {
            public int NoteId { get; set; }
            public int Version { get; set; }
        }

        public class Response { }

        public class Handler : IRequestHandler<Request,Response>
        {
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var note = await _context.Notes.FindAsync(request.NoteId);
                _context.Notes.Remove(note);
                note.RaiseDomainEvent(new NoteRemovedDomainEvent(note.NoteId));
                await _context.SaveChangesAsync(cancellationToken);
                return new Response() { };
            }
        }
    }
}