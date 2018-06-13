using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Features.Identity
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator) => _mediator = mediator;
        
        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<ActionResult<AuthenticateCommand.Response>> SignIn(AuthenticateCommand.Request request)
            => await _mediator.Send(request);

        [AllowAnonymous]
        [HttpGet("signout/{username}")]
        public async Task<ActionResult<SignOutCommand.Response>> SignOut([FromRoute]SignOutCommand.Request request)
            => await _mediator.Send(request);

    }
}
