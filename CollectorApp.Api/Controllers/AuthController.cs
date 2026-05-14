using System;
using System.Web.Http;
using CollectorApp.Api.Interfaces;
using CollectorApp.Api.Models;

namespace CollectorApp.Api.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                if (_authService.Authenticate(model))
                {
                    var response = _authService.GenerateToken($"{model.Name} {model.Surname}".Trim());

                    return Ok(response);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}