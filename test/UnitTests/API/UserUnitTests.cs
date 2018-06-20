using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Moq;
using NoteTakingApp.API.Features.Identity;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Identity;
using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Infrastructure.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.API
{
    public class UserUnitTests
    {
        private readonly Mock<IDistributedCache> _distributedCacheMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<ITokenManager> _tokenManagerMock;
        private readonly Mock<IOptionsSnapshot<AuthenticationSettings>> _authenticationSettingMock;
        private readonly Mock<IMediator> _mediatorMock;

        public UserUnitTests()
        {
            _distributedCacheMock = new Mock<IDistributedCache>();
            _mediatorMock = new Mock<IMediator>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _tokenManagerMock = new Mock<ITokenManager>();
            _authenticationSettingMock = new Mock<IOptionsSnapshot<AuthenticationSettings>>();

            _authenticationSettingMock.Setup(a => a.Value).Returns(new AuthenticationSettings() { MaximumUsers = 2 });

            _mediatorMock
                .Setup(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<byte[]>(), "password"))
                .Returns("password");

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<byte[]>(),"changePassword"))
                .Returns("passwordChanged");

            _tokenManagerMock.Setup(x => x.Issue("quinntynebrown@gmail.com")).Returns("token");

            _tokenManagerMock.Setup(x => x.GetValidToDateTime(It.IsAny<string>())).Returns(DateTime.UtcNow);
        }

        [Fact]
        public async Task ShouldHandleAuthenticateUserCommandRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleAuthenticateUserCommandRequest")
                .Options;

            using (var context = new AppDbContext(options, _mediatorMock.Object))
            {                
                context.Users.Add(new User()
                {
                    Username = "quinntynebrown@gmail.com",
                    Password = "password"
                });

                context.SaveChanges();
                
                var handler = new AuthenticateCommand.Handler(new AccessTokenRepository(context,_distributedCacheMock.Object), context, _authenticationSettingMock.Object, _passwordHasherMock.Object, _tokenManagerMock.Object);

                var response = await handler.Handle(new AuthenticateCommand.Request()
                {
                    Username = "quinntynebrown@gmail.com",
                    Password = "password"
                }, default(CancellationToken));

                Assert.Equal("token", response.AccessToken);
            }
        }
        
    }
}
