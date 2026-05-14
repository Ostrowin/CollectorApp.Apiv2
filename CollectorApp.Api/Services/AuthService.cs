using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using CollectorApp.Api.Dtos;
using CollectorApp.Api.Infrastructure;
using CollectorApp.Api.Interfaces;
using CollectorApp.Api.Models;
using InsERT;
using Microsoft.IdentityModel.Tokens;
using Serilog;



namespace CollectorApp.Api.Services
{
    public class AuthService : IAuthService
    {
        public bool Authenticate(LoginModel model)
        {
            Log.Information("Próba logowania użytkownika: {Surname} {Name}", model.Surname, model.Name);

            if (model == null || string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Surname))
                return false;

            bool isAuthenticated = false;
            string fullName = $"{model.Surname} {model.Name}".Trim();

            var thread = new System.Threading.Thread(() =>
            {
                GT gtTest = null;
                object subiektTest = null;

                try
                {
                    gtTest = new GT();
                    gtTest.Produkt = ProduktEnum.gtaProduktSubiekt;
                    gtTest.Serwer = ConfigurationManager.AppSettings["SubiektServer"];
                    gtTest.Baza = ConfigurationManager.AppSettings["SubiektDatabase"];
                    gtTest.Autentykacja = AutentykacjaEnum.gtaAutentykacjaMieszana;
                    gtTest.Uzytkownik = ConfigurationManager.AppSettings["SubiektUser"];
                    gtTest.UzytkownikHaslo = ConfigurationManager.AppSettings["SubiektPass"];

                    gtTest.Operator = fullName;
                    gtTest.OperatorHaslo = model.Password;

                    subiektTest = (Subiekt)gtTest.Uruchom(1, 4);

                    if (subiektTest != null)
                    {
                        Log.Information("Użytkownik {FullName} uwierzytelniony poprawnie w Subiekcie.", fullName);
                        isAuthenticated = true;
                        var s = (Subiekt)subiektTest;
                        s.Zakoncz();
                        Marshal.ReleaseComObject(s);
                    } 
                    else
                    {
                        Log.Warning("Nieudana próba logowania dla: {FullName}", fullName);
                    }
                }
                catch (Exception ex)
                {

                    Log.Error(ex, "Wystąpił błąd podczas próby logowania użytkownika: {FullName}", fullName);
                    isAuthenticated = false;
                }
                finally
                {
                    if (gtTest != null) Marshal.ReleaseComObject(gtTest);
                }
            });

            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
            thread.Join();

            return isAuthenticated;
        }

        public AuthResponseDto GenerateToken(string fullName)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigHelper.GetSetting("JwtKey", "JWT_KEY_COLLECTOR")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var days = int.Parse(ConfigurationManager.AppSettings["JwtExpireDays"]);
            var expiration = DateTime.Now.AddDays(days);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, fullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: ConfigurationManager.AppSettings["JwtIssuer"],
                audience: ConfigurationManager.AppSettings["JwtAudience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                UserName = fullName
            };
        }
    }
}