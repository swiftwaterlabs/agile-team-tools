using AgileTeamTools.Blazor.Ui;
using AgileTeamTools.Blazor.Ui.Configuration;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

DependencyInjectionConfiguration.Configure(builder.Services,builder.Configuration);

await builder.Build().RunAsync();
