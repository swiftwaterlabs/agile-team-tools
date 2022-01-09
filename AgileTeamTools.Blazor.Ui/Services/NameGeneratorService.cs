using System.Net.Http.Json;

namespace AgileTeamTools.Blazor.Ui.Services
{
    public class NameGeneratorService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NameGeneratorService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetRandomName()
        {
            try
            {
                var url = "https://namey.muffinlabs.com/name.json?frequency=rare&with_surname=true";
                var client = _httpClientFactory.CreateClient();

                var response = await client.GetFromJsonAsync<string[]>(url);

                return response.FirstOrDefault() ?? Guid.NewGuid().ToString();
            }
            catch
            {
                return Guid.NewGuid().ToString();
            }
            
        }

    }
}
