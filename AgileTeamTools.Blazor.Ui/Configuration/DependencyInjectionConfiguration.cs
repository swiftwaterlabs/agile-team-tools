using AgileTeamTools.Blazor.Ui.Services;
using Microsoft.AspNetCore.SignalR.Client;

namespace AgileTeamTools.Blazor.Ui.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var apiUrl = configuration["Api_Url"];

            services.AddHttpClient("Api",client =>client.BaseAddress = new Uri(apiUrl));
            services.AddTransient<BroadcastService>();

            ConfigureSignalR(services,apiUrl);
        }

        private static void ConfigureSignalR(IServiceCollection services,string apiUrl)
        {
            services.AddTransient(provider =>
            {
                var hubConnection = new HubConnectionBuilder()
                    .WithUrl(apiUrl)
                    .Build();

                return hubConnection;
            });
        }
    }
}
