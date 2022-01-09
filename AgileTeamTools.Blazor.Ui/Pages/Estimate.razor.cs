using AgileTeamTools.Blazor.Ui.Models;
using AgileTeamTools.Blazor.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace AgileTeamTools.Blazor.Ui.Pages
{
    public partial class Estimate:IAsyncDisposable
    {
        [Inject] 
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public HubConnection HubConnection { get; set; }
        [Inject]
        public BroadcastService BroadcastService { get; set; }

        [Parameter]
        public string TeamId { get; set; }

        private List<string> messages = new List<string>();
        private string? userInput;
        private string? messageInput;

        protected override async Task OnInitializedAsync()
        {
            HubConnection.On<string, string>($"broadcast|{TeamId}|estimate", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                messages.Add(encodedMsg);
                StateHasChanged();
            });

            await HubConnection.StartAsync();
        }

        private Task Send()
        {
            return BroadcastService.BroadcastAsync(TeamId, "estimate", new Message { UserName = userInput, Body = messageInput });
        }

        public bool IsConnected =>
            HubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (HubConnection is not null)
            {
                await HubConnection.DisposeAsync();
            }
        }
    }
}
