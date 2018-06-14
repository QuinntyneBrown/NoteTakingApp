using FluentValidation;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Identity;
using NoteTakingApp.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
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
            private readonly IAppDbContext _context;
            private readonly IOptions<AuthenticationSettings> _authenticationSettings;
            private readonly IPasswordHasher _passwordHasher;
            private readonly ITokenManager _tokenManager;
  
            public Handler(IAppDbContext context, IOptions<AuthenticationSettings> authenticationSettings, IPasswordHasher passwordHasher, ITokenManager tokenManager)
            {
                _authenticationSettings = authenticationSettings;
                _context = context;                
                _passwordHasher = passwordHasher;
                _tokenManager = tokenManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {                
                var user = await _context.Users
                    .SingleOrDefaultAsync(x => x.Username.ToLower() == request.Username.ToLower());

                if (user == null)
                    throw new DomainException();

                if (!ValidateUser(user, _passwordHasher.HashPassword(user.Salt, request.Password)))
                    throw new DomainException();

                var validAccessTokens = _context.AccessTokens.Where(x => x.ValidTo > DateTime.UtcNow);

                if (validAccessTokens.Count() >= _authenticationSettings.Value.MaximumUsers)
                    throw new DomainException();

                if (validAccessTokens.Where(x => x.Username == request.Username).SingleOrDefault() != null)
                    throw new DomainException();

                var accessToken = _tokenManager.Issue(request.Username);

                _context.AccessTokens.Add(AccessToken.Create(accessToken, request.Username, _tokenManager.GetValidToDateTime(accessToken)));

                await _context.SaveChangesAsync(cancellationToken);

                return new Response()
                {
                    AccessToken = accessToken,
                    UserId = user.UserId
                };
            }

            public bool ValidateUser(User user, string transformedPassword)
            {
                if (user == null || transformedPassword == null)
                    return false;

                return user.Password == transformedPassword;
            }
            
        }
    }
}