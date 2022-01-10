using AgileTeamTools.Api.Extensions;
using AgileTeamTools.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Threading.Tasks;

namespace AgileTeamTools.Api.Functions
{
    public class MessageFunctions
    {
        [FunctionName("negotiate")]
        public SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "messages")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("broadcast")]
        public async Task<IActionResult> Broadcast(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",Route ="broadcast/{teamId}/{channelId}")] HttpRequest req,
            string teamId,
            string channelId,
            [SignalR(HubName = "messages")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var message = req.Content<Message>();

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = $"broadcast|{teamId}|{channelId}",
                    Arguments = new object[] { message?.UserName,message}
                });

            return new OkResult();
        }

        
    }
}
