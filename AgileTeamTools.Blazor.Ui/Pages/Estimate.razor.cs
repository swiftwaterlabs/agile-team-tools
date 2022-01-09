using AgileTeamTools.Blazor.Ui.Models;
using AgileTeamTools.Blazor.Ui.Repostories;
using AgileTeamTools.Blazor.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace AgileTeamTools.Blazor.Ui.Pages
{
    public partial class Estimate:IAsyncDisposable
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject] 
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public HubConnection HubConnection { get; set; }

        [Inject]
        public BroadcastService BroadcastService { get; set; }

        [Inject]
        public NameGeneratorService NameService { get; set; }

        [Inject]
        public EstimateOptionRepository EstimateOptionRepository { get; set; }

        [Parameter]
        public string TeamId { get; set; }

        private const string ChannelName = "Estimate";

        public string UserName;
        public string EstimatedValue;
        public List<string> EstimateOptions = new();

        public bool IsSubmitted = false;
        public bool AreMessagesVisible = false;
        public Dictionary<string, Message> Estimates = new();

        protected override async Task OnInitializedAsync()
        {
            await ConfigureSignalR();
        }

        protected override async Task OnParametersSetAsync()
        {
            AppState.SetBreadcrumbs(new BreadcrumbItem("Estimate", Paths.Estimate(TeamId)));

            EstimateOptions = EstimateOptionRepository.Get();
            UserName = await NameService.GetRandomName();
        }

        private async Task ConfigureSignalR()
        {
            HubConnection.On($"broadcast|{TeamId}|{ChannelName}", (Action<string, Message>)((user, message) =>
            {
                HandleReceivedMessage(user, message);
            }));

            await HubConnection.StartAsync();
        }

        private void HandleReceivedMessage(string user, Message message)
        {
            switch(message?.Action)
            {
                case "Submit":
                    {
                        if(Estimates.ContainsKey(user))
                        {
                            Estimates[user] = message;
                        }
                        else
                        {
                            Estimates.Add(user, message);
                        }
                        
                        break;
                    }
                case "Hide":
                    {
                        AreMessagesVisible = false;
                        break;
                    }
                case "Show":
                    {
                        AreMessagesVisible = true;
                        break;
                    }
                case "Reset":
                    {
                        Estimates.Clear();
                        break;
                    }
            }
        }

        private void SetEstimate(string estimate)
        {
            EstimatedValue = estimate;
        }

        private Variant GetButtonType(string value)
        {
            if (EstimatedValue == value) return Variant.Filled;
            return Variant.Outlined;
        }

        private Task SubmitEstimate()
        {
            return BroadcastService.BroadcastAsync(TeamId, ChannelName, new Message { UserName = UserName, Body = EstimatedValue, Action="Submit" });
        }

        private Task Show()
        {
            return BroadcastService.BroadcastAsync(TeamId, ChannelName, new Message { UserName = UserName, Body = "", Action = "Show" });
        }

        private Task Hide()
        {
            return BroadcastService.BroadcastAsync(TeamId, ChannelName, new Message { UserName = UserName, Body = "", Action = "Hide" });
        }

        private Task Reset()
        {
            return BroadcastService.BroadcastAsync(TeamId, ChannelName, new Message { UserName = UserName, Body = "", Action = "Reset" });
        }

        public bool IsConnected => HubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (HubConnection is not null)
            {
                await HubConnection.DisposeAsync();
            }
        }
    }
}
