using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;

namespace AgileTeamTools.Ui.Services
{
    public class AgileTeamToolsHub:Hub
    {
        public const string HubUrl = "/chat";

        public Task JoinGroup(string groupName)
        {
            return this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
        }

        public async Task Broadcast(string group, string username, string message)
        {
            await Clients.Group(group).SendAsync("Broadcast", username, message);
        }
    }
}
