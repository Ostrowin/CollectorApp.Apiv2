using Serilog;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CollectorApp.Api.Infrastructure
{
    public class RequestResponseLogger : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid().ToString().Substring(0, 8);
            var requestBody = "brak";

            if (request.Content != null)
            {
                requestBody = await request.Content.ReadAsStringAsync();
            }

            Log.Information("[{ID}] HTTP {Method} {Url} | Body: {Body}",
                correlationId, request.Method, request.RequestUri, requestBody);

            var response = await base.SendAsync(request, cancellationToken);

            var responseBody = "brak";
            if (response.Content != null)
            {
                responseBody = await response.Content.ReadAsStringAsync();
                if (responseBody.Length > 500) responseBody = responseBody.Substring(0, 500) + "... [skrócono]";
            }

            Log.Information("[{ID}] Odpowiedź: {StatusCode} | Body: {Body}",
                correlationId, response.StatusCode, responseBody);

            return response;
        }
    }
}