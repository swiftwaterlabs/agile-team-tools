using AgileTeamTools.Api.Test.Testing.Fakes;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AgileTeamTools.Api.Test.Testing.Extensions
{
    public static class HttpExtensions
    {
        public static HttpRequest GetRequest(this object root)
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext()) { Method = "GET" };
            return request;
        }

        public static HttpRequest PostRequest<T>(this object root, T body)
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext()) { Method = "POST" };
            var bodyAsJson = JsonConvert.SerializeObject(body);
            request.Body = bodyAsJson.ToStream();

            return request;
        }
    }
}
