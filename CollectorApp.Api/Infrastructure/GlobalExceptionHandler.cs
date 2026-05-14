using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using CollectorApp.Api.Models;

namespace CollectorApp.Api.Infrastructure
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var response = new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Wystąpił nieoczekiwany błąd serwera. Skontaktuj się z administratorem.",
                Details = context.Exception.Message
            };

            context.Result = new ErrorResult
            {
                Request = context.ExceptionContext.Request,
                Content = response
            };
        }

        private class ErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }
            public ErrorResponse Content { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = Request.CreateResponse((HttpStatusCode)Content.StatusCode, Content);
                return Task.FromResult(response);
            }
        }
    }
}