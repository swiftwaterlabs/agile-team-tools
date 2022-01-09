using AgileTeamTools.Ui.Hubs;
using AgileTeamTools.Ui.Models;
using AgileTeamTools.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgileTeamTools.Ui.Pages
{
    public partial class Estimate
    {
        private HubConnection _hubConnection;

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        NameGeneratorService NameService { get; set; }

        [Parameter]
        public string TeamId { get; set; }
        private string ChannelName = "Estimate";

        public string UserName { get; set; }
        public string EstimatedValue { get; set; }
        public bool IsSubmitted { get; set; } = false;
        public List<string> EstimateOptions = new List<string>
        {
            "1",
            "2",
            "3",
            "5",
            "8",
            "13",
            "21",
            "?",
        };

        public bool AreMessagesVisible = false;
        public Dictionary<string,Message> Estimates = new Dictionary<string, Message>();

        protected override async Task OnInitializedAsync()
        {
            UserName = await NameService.GetRandomName();
        }

        protected override Task OnParametersSetAsync()
        {

            return Start();
        }

        public async Task Start()
        {
            var baseUrl = NavigationManager.BaseUri;
            var hubUrl = baseUrl.TrimEnd('/') + AgileTeamToolsHub.HubUrl;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            _hubConnection.On<string, string>(AgileTeamToolsHub.MethodNames.Broadcast, HandleMessageReceived);
            _hubConnection.On(AgileTeamToolsHub.MethodNames.Reset, HandleReset);
            _hubConnection.On(AgileTeamToolsHub.MethodNames.Show, HandleShow);
            _hubConnection.On(AgileTeamToolsHub.MethodNames.Hide, HandleHide);

            await _hubConnection.StartAsync();

            await _hubConnection.SendAsync(AgileTeamToolsHub.MethodNames.JoinGroup, TeamId, ChannelName);
            
        }

        private void HandleMessageReceived(string name, string message)
        {
            if(!Estimates.ContainsKey(name))
            {
                Estimates.Add(name, new Message(name, message));
            }
            Estimates[name].Body = message;

            StateHasChanged();
        }

        private void HandleReset()
        {
            EstimatedValue = null;
            Estimates.Clear();
            AreMessagesVisible = false;
            StateHasChanged();
        }

        private void HandleShow()
        {
            AreMessagesVisible = true;
            StateHasChanged();
        }

        private void HandleHide()
        {
            AreMessagesVisible = false;
            StateHasChanged();
        }

        private async Task Submit()
        {
            await _hubConnection.SendAsync(AgileTeamToolsHub.MethodNames.Broadcast, TeamId, ChannelName, UserName, EstimatedValue);
            IsSubmitted = true;
        }

        private async Task Reset()
        {
            await _hubConnection.SendAsync(AgileTeamToolsHub.MethodNames.Reset, TeamId, ChannelName);
            IsSubmitted = false;
        }

        private async Task Show()
        {
            await _hubConnection.SendAsync(AgileTeamToolsHub.MethodNames.Show, TeamId, ChannelName);
        }

        private async Task Hide()
        {
            await _hubConnection.SendAsync(AgileTeamToolsHub.MethodNames.Hide, TeamId, ChannelName);
        }

        private void SetEstimate(string value)
        {
            EstimatedValue = value;
        }
    }
}
