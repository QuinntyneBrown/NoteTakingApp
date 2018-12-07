using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static NoteTakingApp.API.Features.Identity.AuthenticateCommand;

namespace NoteTakingApp.API.Features.Identity
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/users")]
    public class UsersController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator) {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpPost("token")]
        public async Task<ActionResult<Response>> SignIn(Request request)
            => await _mediator.Send(request);

        [HttpGet("signout/{username}")]
        public IActionResult SignOut([FromRoute]SignOutCommand.Request request)
            => new OkObjectResult(null);
    }
}
