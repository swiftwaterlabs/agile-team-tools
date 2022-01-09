﻿using AgileTeamTools.Blazor.Ui.Models;
using AgileTeamTools.Blazor.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Concurrent;

namespace AgileTeamTools.Blazor.Ui.Pages
{
    public partial class LeanCoffee:IAsyncDisposable
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public HubConnection HubConnection { get; set; }

        [Inject]
        public BroadcastService BroadcastService { get; set; }

        [Inject]
        public NameGeneratorService NameService { get; set; }

        [Parameter]
        public string TeamId { get; set; } = "";

        private const string ChannelName = "LeanCoffee";

        public string UserName = "";

        public bool IsSubmitted = false;
        public bool AreMessagesVisible = false;
        public bool IsTimerVisible = false;
        public ConcurrentDictionary<string, Message> Results = new();
        public string TimerValue = "00:00:00";

        protected override async Task OnInitializedAsync()
        {
            await ConfigureSignalR();
        }

        protected override async Task OnParametersSetAsync()
        {
            AppState.SetBreadcrumbs(new BreadcrumbItem("Lean Coffee", Paths.Estimate(TeamId)));

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
                case Actions.StartTimer:
                    {
                        HandleStartTimerMessage();
                        break;
                    }
                case Actions.StopTimer:
                    {
                        HandleStopTimerMessage();
                        break;
                    }
                case Actions.ResetTimer:
                    {
                        HandleResetTimerMessage();
                        break;
                    }
                case Actions.TimerTick:
                    {
                        HandleTimerTickMessage(message);
                        break;
                    }
            }
        }

        private void HandleSubmitMessage(string user, Message message)
        {
            Results.AddOrUpdate(user, message, (key, existing) => 
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
            Results.Clear();
        }

        private void HandleStartTimerMessage()
        {
            
        }

        private void HandleStopTimerMessage()
        {

        }

        private void HandleResetTimerMessage()
        {

        }

        private void HandleTimerTickMessage(Message message)
        {
            throw new NotImplementedException();
        }

        private Task SubmitYes()
        {
            return SendMessage(Actions.Submit,"Yes");
        }

        private Task SubmitNo()
        {
            return SendMessage(Actions.Submit, "No");
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

        private Task StartTimer()
        {
            return SendMessage(Actions.StartTimer);
        }

        private Task StopTimer()
        {
            return SendMessage(Actions.StopTimer);
        }

        private Task ResetTimer()
        {
            return SendMessage(Actions.ResetTimer);
        }

        private Task SendMessage(string action, string body="")
        {
            return BroadcastService.BroadcastAsync(TeamId, ChannelName, CreateMessage(action,body));
        }

        private Message CreateMessage(string action, string body)
        {
            return new Message { UserName = UserName, Body = body, Action = action };
        }

        private string GetResultIcon(string value)
        {
            switch (value)
            {
                case "Yes": return Icons.Material.Outlined.ThumbUp;
                case "No": return Icons.Material.Outlined.ThumbDown;
                default: return Icons.Material.Outlined.QuestionMark;
            }
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
