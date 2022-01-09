﻿using AgileTeamTools.Blazor.Ui.Models;
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

        private List<string> messages = new();
        private string? userInput;
        private string? messageInput;

        protected override async Task OnInitializedAsync()
        {
            HubConnection.On<string, string>($"broadcast|{TeamId}|{ChannelName}", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                messages.Add(encodedMsg);
                StateHasChanged();
            });

            await HubConnection.StartAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            AppState.SetBreadcrumbs(new BreadcrumbItem("Estimate", Paths.Estimate(TeamId)));

            EstimateOptions = EstimateOptionRepository.Get();
            UserName = await NameService.GetRandomName();
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
            return Task.CompletedTask;
        }

        private Task Show()
        {
            return Task.CompletedTask;
        }

        private Task Hide()
        {
            return Task.CompletedTask;
        }

        private Task Reset()
        {
            Estimates.Clear();

            return Task.CompletedTask;
        }

        private Task Send()
        {
            return BroadcastService.BroadcastAsync(TeamId, ChannelName, new Message { UserName = userInput, Body = messageInput });
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