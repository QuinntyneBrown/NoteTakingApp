using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Infrastructure
{
    public class AccessTokenRepositoryTests
    {
        private readonly AccessTokenRepository _accessTokenRepository;
        private readonly IAppDbContext _context;
        private readonly Mock<IDistributedCache> _distributedCacheMock;
        public AccessTokenRepositoryTests()
        {
            _distributedCacheMock = new Mock<IDistributedCache>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AccessTokenRepositoryTests")
                .Options;


            _context = new AppDbContext(options);
            _accessTokenRepository = new AccessTokenRepository(_context,_distributedCacheMock.Object);
        }

        [Fact]
        public void ShouldNotBeNull() {

            Assert.NotNull(_accessTokenRepository);
        }


        [Fact]
        public async Task ShouldGetValidAccessTokens()
        {

            await _context.AccessTokens.AddAsync(new NoteTakingApp.Core.Entities.AccessToken()
            {
                ValidTo = DateTime.UtcNow.AddYears(10),
                Value = "",
                Username = "Test"
            });

            await _context.SaveChangesAsync(default(CancellationToken));

            var accessTokens = await _accessTokenRepository.GetValidAccessTokenValuesAsync();

            Assert.Single(accessTokens);            
        }
    }
}
