using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Infrastructure.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Infrastructure
{
    public class AccessTokenRepositoryTests
    {
        private readonly AccessTokenRepository _accessTokenRepository;
        private readonly IAppDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public AccessTokenRepositoryTests()
        {
            _distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AccessTokenRepositoryTests")
                .Options;


            _context = new AppDbContext(options);
            _accessTokenRepository = new AccessTokenRepository(_context, _distributedCache);
        }

        [Fact]
        public void ShouldNotBeNull() {
            Assert.NotNull(_accessTokenRepository);
        }
        
        [Fact]
        public async Task ShouldGetValidAccessTokens()
        {
            await _context.AccessTokens.AddAsync(new NoteTakingApp.Core.Models.AccessToken()
            {
                Value = "",
                Username = "Test"
            });

            await _context.SaveChangesAsync(default(CancellationToken));

            var accessTokens = await _accessTokenRepository.GetValidAccessTokenValuesAsync();

            Assert.Single(accessTokens);            
        }
    }
}
