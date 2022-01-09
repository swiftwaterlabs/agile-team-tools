using AgileTeamTools.Blazor.Ui.Models;
using AgileTeamTools.Blazor.Ui.Repostories;
using AgileTeamTools.Blazor.Ui.Services;
using BlazorApplicationInsights;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Services;

namespace AgileTeamTools.Blazor.Ui.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var apiUrl = configuration["Api_Url"];

            services.AddSingleton<AppState>();

            services.AddTransient<EstimateOptionRepository>();

            services.AddTransient<NameGeneratorService>();
            

            ConfigureApplicationInsights(services, configuration);
            ConfigureApi(services, apiUrl);
            ConfigureUi(services);
            ConfigureSignalR(services, apiUrl);
        }

        private static void ConfigureApplicationInsights(IServiceCollection services, IConfiguration configuration)
        {
            services.AddBlazorApplicationInsights(async applicationInsights =>
            {
                var instrumentationKey = configuration["AppInsights_InstrumentationKey"];
                await applicationInsights.SetInstrumentationKey(instrumentationKey);
                await applicationInsights.LoadAppInsights();
            });
        }

        private static void ConfigureUi(IServiceCollection services)
        {
            services.AddMudServices();
        }

        private static void ConfigureApi(IServiceCollection services, string apiUrl)
        {
            services.AddHttpClient("Api", client => client.BaseAddress = new Uri(apiUrl));
            services.AddTransient<BroadcastService>();
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
