using Serilog;
using System.Web.Http.ExceptionHandling;

namespace CollectorApp.Api.Infrastructure
{
    public class GlobalExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            var request = context.Request;
            var method = request.Method.ToString();
            var url = request.RequestUri.ToString();

            Serilog.Log.Error(context.Exception,
                "Nieobsłużony wyjątek w WebAPI! Metoda: {Method}, URL: {Url}",
                method, url);
        }
    }
}