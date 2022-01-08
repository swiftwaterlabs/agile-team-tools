using AgileTeamTools.Api.Extensions;
using AgileTeamTools.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AgileTeamTools.Api.Functions
{
    public static class MessageFunctions
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "messages")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("broadcast")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",Route ="broadcast/{teamId}/{channelId}")] HttpRequest req,
            string teamId,
            string channelId,
            [SignalR(HubName = "messages")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var message = req.Content<Message>();

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = $"NewMessage",
                    Arguments = new[] { teamId, channelId, message.UserName,message.Body}
                });

            return new OkResult();
        }

        
    }
}