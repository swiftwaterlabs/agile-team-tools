using AgileTeamTools.Ui.Models;
using AgileTeamTools.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgileTeamTools.Ui.Pages
{
    public partial class Estimate
    {
        private string _hubUrl;
        private HubConnection _hubConnection;

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string TeamId { get; set; }
        private string ChannelName = "Estimate";

        public string UserName { get; set; }
        public string EstimatedValue { get; set; }

        public bool AreMessagesVisible = false;
        public List<Message> Messages = new List<Message>();

        protected override Task OnParametersSetAsync()
        {
            return Start();
        }

        public async Task Start()
        {
            var baseUrl = NavigationManager.BaseUri;
            _hubUrl = baseUrl.TrimEnd('/') + AgileTeamToolsHub.HubUrl;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
                .Build();

            _hubConnection.On<string, string>("Broadcast", ReceiveMessage);
            
            await _hubConnection.StartAsync();

            await _hubConnection.SendAsync("JoinGroup", TeamId, ChannelName);
            
        }

        private void ReceiveMessage(string name, string message)
        {
            bool isMine = name.Equals(UserName, StringComparison.OrdinalIgnoreCase);

            Messages.Add(new Message(name, message, isMine));

            StateHasChanged();
        }

        private async Task Send()
        {
            await _hubConnection.SendAsync("Broadcast",TeamId, ChannelName, UserName, EstimatedValue);
        }

        private void Reset()
        {
            Messages.Clear();
            AreMessagesVisible = false;
        }

        private void Show()
        {
            AreMessagesVisible = true;
        }
    }
}
