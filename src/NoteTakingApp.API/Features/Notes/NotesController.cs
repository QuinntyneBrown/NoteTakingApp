using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Features.Notes
{
    [Authorize]
    [ApiController]
    [Route("api/notes")]
    public class NotesController
    {
        private readonly IMediator _mediator;
        private readonly IConcurrentCommandGuard _concurrentCommandGuard;

        public NotesController(IConcurrentCommandGuard concurrentCommandGuard, IMediator mediator)
        {
            _concurrentCommandGuard = concurrentCommandGuard ?? throw new ArgumentNullException(nameof(concurrentCommandGuard));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<GetNoteBySlugQuery.Response>> GetBySlug([FromRoute]GetNoteBySlugQuery.Request request)
            => await _mediator.Send(request);

        [HttpPost]
        public async Task<ActionResult<SaveNoteCommand.Response>> Save(SaveNoteCommand.Request request)
            => await _concurrentCommandGuard.Process(request, _mediator.Send);

        [HttpDelete("{noteId}/version/{version}")]
        public async Task Remove([FromRoute]RemoveNoteCommand.Request request)
            => await _mediator.Send(request);

        [HttpGet("{noteId}")]
        public async Task<ActionResult<GetNoteByIdQuery.Response>> GetById([FromRoute]GetNoteByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetNotesQuery.Response>> Get()
            => await _mediator.Send(new GetNotesQuery.Request());
    }
}
