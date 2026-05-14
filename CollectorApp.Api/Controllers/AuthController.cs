using System.Web.Http;
using CollectorApp.Api.Models;
using CollectorApp.Api.Interfaces;

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                bool isAuthenticated = _authService.Authenticate(model);

                if (isAuthenticated)
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = "Zalogowano pomyślnie",
                        User = model.FullName
                    });
                }

                return Unauthorized();
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}