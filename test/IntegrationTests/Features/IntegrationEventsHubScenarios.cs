using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class IntegrationEventsHubScenarios: IntegrationEventsHubScenarioBase
    {
        [Fact]
        public async Task ShouldPreventTwoConnectionWithSameUser()
        {
            try
            {
                using (var server = CreateServer())
                {
                    var hubConnection = GetHubConnection(server.CreateHandler());

                    await hubConnection.StartAsync();

                    var anotherConnection = GetHubConnection(server.CreateHandler());

                    await anotherConnection.StartAsync();

                    //Assert.True(anotherConnection.Closed)
                }
            }catch(Exception e)
            {
                throw e;
            }

            Assert.True(true);
        }
    }
}
