using System.Web.Http;
using CollectorApp.Api.Infrastructure;
using CollectorApp.Api.Services;

[RoutePrefix("api/health")]
public class HealthController : ApiController
{
    private readonly HealthCheckService _healthService;

    public HealthController()
    {
        _healthService = new HealthCheckService(new SubiektGTService());
    }

    [HttpGet]
    [Route("")]
    public IHttpActionResult GetStatus()
    {
        var status = _healthService.CheckEverything();

        if (!status.IsDatabaseHealthy || !status.IsSubiektHealthy)
        {
            return Content(System.Net.HttpStatusCode.ServiceUnavailable, status);
        }

        return Ok(status);
    }
}