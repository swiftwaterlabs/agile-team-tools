using AgileTeamTools.Blazor.Ui.Models;
using System.Net.Http.Json;

namespace AgileTeamTools.Blazor.Ui.Services
{
    public class BroadcastService
    {
        private readonly IHttpClientFactory _clientFactory;

        public BroadcastService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public Task BroadcastAsync(string teamId, string channelId, Message message)
        {
            var client = _clientFactory.CreateClient("Api");
            return client.PostAsJsonAsync($"broadcast/{teamId}/{channelId}", message);
        }
    }
}
