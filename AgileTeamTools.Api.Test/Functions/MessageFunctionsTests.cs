using AgileTeamTools.Api.Functions;
using AgileTeamTools.Api.Models;
using AgileTeamTools.Api.Test.Testing.Extensions;
using AgileTeamTools.Api.Test.Testing.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Threading.Tasks;
using Xunit;

namespace AgileTeamTools.Api.Test.Functions
{
    public class MessageFunctionsTests
    {
        [Fact]
        public void Negotiate_ReturnsConnectionInfo()
        {
            var function = new MessageFunctions();
            var connectionInfo = new SignalRConnectionInfo();

            var result = function.Negotiate(this.GetRequest(), connectionInfo);

            Assert.Same(result, connectionInfo);
        }

        [Fact]
        public async Task Broacast_ValidChannelIdAndTeamIdAndMessage_SendsMessage()
        {
            var function = new MessageFunctions();

            var teamId = "the-team";
            var channelId = "my-channel";
            var signalRMessages = new AsyncCollectorFake<SignalRMessage>();

            var message = new Message
            {
                UserName = "the-user",
                Body = "what I want to say"
            };

            var result = await function.Broadcast(this.PostRequest(message), teamId, channelId, signalRMessages);

            AssertOkResult(result);
            AssertOneMessageSent(signalRMessages);

            var sentMessage = signalRMessages.Data[0];
            AssertMessageSentToTeamChannel(teamId, channelId, sentMessage);
            AssertMessageSentWithUserName(message, sentMessage);
            AssertMessageSentWithBody(message, sentMessage);
        }

        private static void AssertOkResult(IActionResult result)
        {
            Assert.IsType<OkResult>(result);
        }

        private static void AssertOneMessageSent(AsyncCollectorFake<SignalRMessage> signalRMessages)
        {
            Assert.Single(signalRMessages.Data);
        }

        private static void AssertMessageSentToTeamChannel(string teamId, string channelId, SignalRMessage sentMessage)
        {
            Assert.Equal($"broadcast|{teamId}|{channelId}", sentMessage.Target);
        }

        private static void AssertMessageSentWithUserName(Message message, SignalRMessage sentMessage)
        {
            Assert.Equal(message.UserName, sentMessage.Arguments[0]);
        }

        private static void AssertMessageSentWithBody(Message message, SignalRMessage sentMessage)
        {
            Assert.Equal(message.Body, sentMessage.Arguments[1]);
        }
    }
}
