using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICache _cache;
        private readonly IAppDbContext _context;
        
        public AccessTokenRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AccessTokenRepositoryTests")
                .Options;

            _cache = new NoteTakingApp.Core.MemoryCache(new MemoryCache(new MemoryCacheOptions()));
            _context = new AppDbContext(options);
            _accessTokenRepository = new AccessTokenRepository(_context, _cache);
        }

        [Fact]
        public void ShouldNotBeNull() {

            Assert.NotNull(_accessTokenRepository);
        }

        [Fact]
        public async Task ShouldGetFromCacheOrServiceAsync()
        {

            _cache.Add(new List<string>()
            {
                "Value"
            }, "ValidAccessTokens");

            var accessTokens = await _accessTokenRepository.GetValidAccessTokenValuesAsync();

            Assert.Single(accessTokens);
            Assert.Single(_cache.Get<List<string>>("ValidAccessTokens"));

        }

        [Fact]
        public async Task ShouldInvalidateCacheOnSave()
        {

            _cache.Add(new List<string>()
            {
                "Value"
            }, "ValidAccessTokens");

            var accessTokens = await _accessTokenRepository.GetValidAccessTokenValuesAsync();

            Assert.Single(accessTokens);
            Assert.Single(_cache.Get<List<string>>("ValidAccessTokens"));

            await _accessTokenRepository.SaveChangesAsync(default(CancellationToken));

            Assert.Null(_cache.Get<List<string>>("ValidAccessTokens"));
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
            Assert.Single(_cache.Get<List<string>>("ValidAccessTokens"));

        }
    }
}
