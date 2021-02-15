using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Http;

namespace AgileTeamTools.Ui.Services
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
            var url = "https://namey.muffinlabs.com/name.json?frequency=rare&with_surname=true";
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<string[]>(url);

            return response.FirstOrDefault();
        }

    }
}
