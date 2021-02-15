﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;

namespace AgileTeamTools.Ui.Services
{
    public class AgileTeamToolsHub:Hub
    {
        public const string HubUrl = "/default";

        public Task JoinGroup(string teamId, string channel)
        {
            var groupName = GetGroupName(teamId, channel);
            return this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
        }

        public async Task Broadcast(string teamId, string channel, string username, string message)
        {
            var groupName = GetGroupName(teamId, channel);
            await Clients.Group(groupName).SendAsync("Broadcast", username, message);
        }

        public async Task Reset(string teamId, string channel)
        {
            var groupName = GetGroupName(teamId, channel);
            await Clients.Group(groupName).SendAsync("Reset");
        }

        public async Task Show(string teamId, string channel)
        {
            var groupName = GetGroupName(teamId, channel);
            await Clients.Group(groupName).SendAsync("Show");
        }

        private static string GetGroupName(string teamId, string channel)
        {
            return $"{teamId}|{channel}";
        }
    }
}
