using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NoteTakingApp.Core.Models;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Identity;
using NoteTakingApp.Core.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Features.Identity
{
    public class AuthenticateCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.Username).NotEqual(default(string));
                RuleFor(request => request.Password).NotEqual(default(string));
            }            
        }

        public class Request : IRequest<Response>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Response
        {
            public string AccessToken { get; set; }
            public int UserId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAccessTokenRepository _repository;
            private readonly IAppDbContext _context;
            private readonly IOptionsSnapshot<AuthenticationSettings> _authenticationSettings;
            private readonly IPasswordHasher _passwordHasher;
            private readonly ITokenManager _tokenManager;
  
            public Handler(IAccessTokenRepository repository, IAppDbContext context, IOptionsSnapshot<AuthenticationSettings> authenticationSettings, IPasswordHasher passwordHasher, ITokenManager tokenManager)
            {
                _context = context;
                _authenticationSettings = authenticationSettings;
                _repository = repository;                
                _passwordHasher = passwordHasher;
                _tokenManager = tokenManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {                
                var user = await _context.Users
                    .SingleOrDefaultAsync(x => x.Username.ToLower() == request.Username.ToLower());

                if (user == null) throw new DomainException("Invalid Username!");

                if (_passwordHasher.HashPassword(user.Salt, request.Password) != user.Password)
                    throw new DomainException("Invalid Password!");

                var validAccessTokens = _repository.GetValidAccessTokens();

                if (validAccessTokens.Count() >= _authenticationSettings.Value.MaximumUsers)
                    throw new DomainException("Exceeded Maximum Users!");

                if (validAccessTokens.Where(x => x.Username == request.Username).SingleOrDefault() != null)
                    throw new DomainException("Already logged In!");

                var accessToken = _tokenManager.Issue(request.Username);

                _repository.Add(new AccessToken()
                {
                    Value = accessToken,
                    Username = request.Username,
                    ValidTo = _tokenManager.GetValidToDateTime(accessToken)
                });

                await _repository.SaveChangesAsync(cancellationToken);
                
                return new Response()
                {
                    AccessToken = accessToken,
                    UserId = user.UserId
                };
            }               
        }
    }
}
