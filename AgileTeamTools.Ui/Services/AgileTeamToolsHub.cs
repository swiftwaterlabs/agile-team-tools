using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;

namespace AgileTeamTools.Ui.Services
{
    public class AgileTeamToolsHub:Hub
    {
        public const string HubUrl = "/chat";
        
        public async Task Broadcast(string username, string message)
        {
            await Clients.All.SendAsync("Broadcast", username, message);
        }
    }
}
