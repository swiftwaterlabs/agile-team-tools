using AgileTeamTools.Blazor.Ui.Models;
using AgileTeamTools.Blazor.Ui.Repostories;
using AgileTeamTools.Blazor.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Concurrent;

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
        public string TeamId { get; set; } = "";

        private const string ChannelName = "Estimate";

        public string UserName = "";
        public string EstimatedValue = "";
        public List<string> EstimateOptions = new();

        public bool IsSubmitted = false;
        public bool AreMessagesVisible = false;
        public ConcurrentDictionary<string, Message> Estimates = new();

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
                case Actions.Submit:
                    {
                        HandleSubmitMessage(user, message);
                        break;
                    }
                case Actions.Hide:
                    {
                        HandleHideMessage();
                        break;
                    }
                case Actions.Show:
                    {
                        HandleShowMessage();
                        break;
                    }
                case Actions.Reset:
                    {
                        HandleResetMessage();
                        break;
                    }
            }
        }
        private void HandleSubmitMessage(string user, Message message)
        {
            Estimates.AddOrUpdate(user, message, (key, existing) => 
            {
                existing.Body = message.Body;
                return message;
            });
        }

        private void HandleShowMessage()
        {
            AreMessagesVisible = true;
        }

        private void HandleHideMessage()
        {
            AreMessagesVisible = false;
        }

        private void HandleResetMessage()
        {
            Estimates.Clear();
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
            return SendMessage(Actions.Submit,EstimatedValue);
        }

        private Task Show()
        {
            return SendMessage(Actions.Show);
        }
            
        private Task Hide()
        {
            return SendMessage(Actions.Hide);
        }

        private Task Reset()
        {
            return SendMessage(Actions.Reset);
        }

        private Task SendMessage(string action, string body="")
        {
            return BroadcastService.BroadcastAsync(TeamId, ChannelName, CreateMessage(action,body));
        }

        private Message CreateMessage(string action, string body)
        {
            return new Message { UserName = UserName, Body = body, Action = action };
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
