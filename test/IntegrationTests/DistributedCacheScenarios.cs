using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace IntegrationTests
{
    public class DistributedCacheScenarios : ScenarioBase
    {
        [Fact]
        public void ShouldNotBeNull()
        {
            using (var server = CreateServer())
            {
                var cache = server.Host.Services.GetService(typeof(IDistributedCache)) as IDistributedCache;

                Assert.NotNull(cache);
            }
        }
    }
}
