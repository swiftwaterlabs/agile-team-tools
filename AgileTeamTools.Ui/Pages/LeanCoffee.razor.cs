using AgileTeamTools.Ui.Hubs;
using AgileTeamTools.Ui.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileTeamTools.Ui.Pages
{
    public partial class LeanCoffee
    {
        private HubConnection _timerHubConnection;
        private HubConnection _actionHubConnection;

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string TeamId { get; set; }
        private string TimerChannelName = "LeanCoffee/Timer";
        private string ActionChannelName = "LeanCoffee/Action";

        public int Minutes { get; set; } = 5;
        public string Elapsed { get; set; }

        private System.Timers.Timer TimerObject = new System.Timers.Timer();
        private DateTime ElapsedAt { get; set; }
        private bool IsTimerRunning { get; set; } = false;
        private bool CanManageTimer { get; set; } = true;

        private string SelectedAction { get; set; }
        private bool AreActionsVisible { get; set; } = false;
        public Dictionary<string, Message> Actions = new Dictionary<string, Message>();

        protected override void OnInitialized()
        {
            TimerObject.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            TimerObject.Elapsed += TimerObject_Elapsed;
        }

        protected override Task OnParametersSetAsync()
        {
            return Start();
        }

        public async Task Start()
        {
            var baseUrl = NavigationManager.BaseUri;
            var hubUrl = baseUrl.TrimEnd('/') + AgileTeamToolsHub.HubUrl;

            await ConfigureTimerHub(hubUrl);
            await ConfigureActionHub(hubUrl);
        }

        private async Task ConfigureTimerHub(string hubUrl)
        {
            _timerHubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            _timerHubConnection.On<string, string>(AgileTeamToolsHub.MethodNames.Broadcast, HandleMessageReceived);
            _timerHubConnection.On(AgileTeamToolsHub.MethodNames.Reset, HandleReset);

            await _timerHubConnection.StartAsync();
            await _timerHubConnection.SendAsync(AgileTeamToolsHub.MethodNames.JoinGroup, TeamId, TimerChannelName);
        }

        private async Task ConfigureActionHub(string hubUrl)
        {
            _actionHubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            _actionHubConnection.On<string, string>(AgileTeamToolsHub.MethodNames.Broadcast, HandleActionMessageReceived);
            _actionHubConnection.On(AgileTeamToolsHub.MethodNames.Show, HandleShow);
            _actionHubConnection.On(AgileTeamToolsHub.MethodNames.Hide, HandleHide);

            await _actionHubConnection.StartAsync();
            await _actionHubConnection.SendAsync("JoinGroup", TeamId, ActionChannelName);
        }

        private void HandleMessageReceived(string name, string message)
        {
            CanManageTimer = name == _timerHubConnection.ConnectionId;

            Elapsed = message;
            StateHasChanged();
        }

        private void HandleActionMessageReceived(string name, string message)
        {
            if (!Actions.ContainsKey(name))
            {
                Actions.Add(name, new Message(name, message));
            }
            Actions[name].Body = message;
            StateHasChanged();
        }

        private void HandleReset()
        {
            Elapsed = null;
            SelectedAction = null;
            Actions.Clear();
            AreActionsVisible = false;

            StateHasChanged();
        }

        private async Task StartTimer()
        {
            await ResetTimer();

            TimerObject.Start();
            IsTimerRunning = true;
        }

        private void StopTimer()
        {
            TimerObject.Stop();
            IsTimerRunning = false;
        }

        private Task ResetTimer()
        {
            StopTimer();
            ElapsedAt = DateTime.Now.AddMinutes(Minutes);
            return _timerHubConnection.SendAsync(AgileTeamToolsHub.MethodNames.Reset, TeamId, TimerChannelName);
        }

        private void TimerObject_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var elapsed = ElapsedAt.Subtract(e.SignalTime);
            var elapsedFormatted = $"{elapsed.Minutes}:{elapsed.Seconds.ToString("00")}";
            
            _timerHubConnection.SendAsync(AgileTeamToolsHub.MethodNames.Broadcast, TeamId, TimerChannelName, _timerHubConnection.ConnectionId, elapsedFormatted);
            
        }

        private async Task SetAction(string value)
        {
            SelectedAction = value;
            await _actionHubConnection.SendAsync(AgileTeamToolsHub.MethodNames.Broadcast, TeamId, ActionChannelName, _actionHubConnection.ConnectionId, value);
        }

        public async Task ShowResponses()
        {
            await _actionHubConnection.SendAsync(AgileTeamToolsHub.MethodNames.Show, TeamId, ActionChannelName);
        }

        public async Task HideResponses()
        {
            await _actionHubConnection.SendAsync(AgileTeamToolsHub.MethodNames.Hide, TeamId, ActionChannelName);
        }

        private void HandleShow()
        {
            AreActionsVisible = true;
            StateHasChanged();
        }

        private void HandleHide()
        {
            AreActionsVisible = false;
            StateHasChanged();
        }

        private IDictionary<string,int> GetActionSummary()
        {
            var summary = Actions
                .Values
                .GroupBy(a => a.Body)
                .ToDictionary(g => g.Key, a => a.Count());

            return summary;
        }
    }
}
