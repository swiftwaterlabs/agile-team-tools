using AgileTeamTools.Ui.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace AgileTeamTools.Ui.Pages
{
    public partial class LeanCoffee
    {
        private HubConnection _hubConnection;

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string TeamId { get; set; }
        private string ChannelName = "Timer";

        public int Minutes { get; set; } = 5;
        public string Elapsed { get; set; }

        private System.Timers.Timer TimerObject = new System.Timers.Timer();
        private DateTime ElapsedAt { get; set; }
        private bool IsTimerRunning { get; set; } = false;
        private bool CanManageTimer { get; set; } = true;

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

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            _hubConnection.On<string, string>("Broadcast", HandleMessageReceived);
            _hubConnection.On("Reset", HandleReset);

            await _hubConnection.StartAsync();
            await _hubConnection.SendAsync("JoinGroup", TeamId, ChannelName);

        }

        private void HandleMessageReceived(string name, string message)
        {
            CanManageTimer = name == _hubConnection.ConnectionId;

            Elapsed = message;
            StateHasChanged();
        }

        private void HandleReset()
        {
            Elapsed = null;
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
            return _hubConnection.SendAsync("Reset", TeamId, ChannelName);
        }

        private void TimerObject_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var elapsed = ElapsedAt.Subtract(e.SignalTime);
            var elapsedFormatted = $"{elapsed.Minutes}:{elapsed.Seconds.ToString("00")}";
            
            _hubConnection.SendAsync("Broadcast", TeamId, ChannelName, _hubConnection.ConnectionId, elapsedFormatted);
            
        }
    }
}
