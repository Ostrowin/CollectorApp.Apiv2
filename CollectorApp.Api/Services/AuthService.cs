using System;
using System.Configuration;
using System.Runtime.InteropServices;
using CollectorApp.Api.Interfaces;
using CollectorApp.Api.Models;
using InsERT;

namespace CollectorApp.Api.Services
{
    public class AuthService : IAuthService
    {
        public bool Authenticate(LoginModel model)
        {
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
                        isAuthenticated = true;
                        var s = (Subiekt)subiektTest;
                        s.Zakoncz();
                        Marshal.ReleaseComObject(s);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Authentication error: {ex.StackTrace}");
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
    }
}