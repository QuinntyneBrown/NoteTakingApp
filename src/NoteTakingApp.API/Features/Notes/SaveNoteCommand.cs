using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.DomainEvents;
using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Features.Notes
{
    public class SaveNoteCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.Note.NoteId).NotNull();
            }
        }

        public class Request : IRequest<Response>,ICommand<Response>
        {
            public NoteApiModel Note { get; set; }

            public string Key {
                get
                {
                    return Note.NoteId == 0 ? $"{Guid.NewGuid()}" : $"Note-{Note.NoteId}-{Note.Version}";
                }
            }

            public IEnumerable<string> SideEffects => new string[0];
        }

        public class Response
        {
            public int NoteId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var note = await _context.Notes
                    .Include(x => x.NoteTags)
                    .ThenInclude(nt => nt.Tag)
                    .SingleOrDefaultAsync(x => request.Note.NoteId == x.NoteId, cancellationToken);

                if (note == null) _context.Notes.Add(note = new Note());

                note.Update(
                    request.Note.Title,
                    request.Note.Body,
                    request.Note.Tags.Select(x => _context.Tags.Find(x.TagId)).ToList(),
                    request.Note.Version);

                note.RaiseDomainEvent(new NoteSavedDomainEvent(note));

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { NoteId = note.NoteId };
            }
        }
    }
}
