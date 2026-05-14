using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CollectorApp.Api.Services
{
    public class JwtHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;

            if (!TryRetrieveToken(request, out token))
            {
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                var secretKey = ConfigurationManager.AppSettings["JwtKey"];
                var issuer = ConfigurationManager.AppSettings["JwtIssuer"];
                var audience = ConfigurationManager.AppSettings["JwtAudience"];
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                var handler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey
                };

                SecurityToken securityToken;
                var principal = handler.ValidateToken(token, validationParameters, out securityToken);

                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException) { statusCode = HttpStatusCode.Unauthorized; }
            catch (Exception) { statusCode = HttpStatusCode.InternalServerError; }

            return Task<HttpResponseMessage>.Factory.StartNew(() => request.CreateResponse(statusCode));
        }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1) return false;

            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }
}