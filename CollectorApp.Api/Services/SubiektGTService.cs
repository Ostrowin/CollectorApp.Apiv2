//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Threading;
//using System.Web;

//namespace CollectorApp.Api.Services
//{
//    public class SubiektGTService : IDisposable
//    {
//        private readonly IConfiguration _configuration;
//        private Subiekt? _subiekt;

//        public SubiektGTService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public T Execute<T>(Func<Subiekt, T> action)
//        {
//            T result = default!;
//            Exception? exception = null;

//            var thread = new Thread(() =>
//            {
//                try
//                {
//                    var gt = new GT();
//                    gt.Produkt = ProduktEnum.gtaProduktSubiekt;
//                    gt.Serwer = _configuration["Subiekt:Server"];
//                    gt.Baza = _configuration["Subiekt:Database"];
//                    gt.Autentykacja = AutentykacjaEnum.gtaAutentykacjaMieszana;
//                    gt.Uzytkownik = _configuration["Subiekt:Username"];
//                    gt.UzytkownikHaslo = _configuration["Subiekt:Password"];
//                    gt.Operator = _configuration["Subiekt:Operator"];
//                    gt.OperatorHaslo = _configuration["Subiekt:OperatorPassword"];

//                    _subiekt = (Subiekt)gt.Uruchom(
//                        (int)UruchomDopasujEnum.gtaUruchomDopasuj,
//                        (int)UruchomEnum.gtaUruchomNieArchiwizujPrzyZamykaniu
//                    );
//                    _subiekt.Okno.Widoczne = false;

//                    result = action(_subiekt);
//                }
//                catch (Exception ex)
//                {
//                    exception = ex;
//                }
//            });

//            thread.SetApartmentState(ApartmentState.STA);
//            thread.Start();
//            thread.Join();

//            if (exception != null)
//            {
//                throw exception;
//            }

//            return result;
//        }

//        public void Dispose()
//        {
//            if (_subiekt != null)
//            {
//                Marshal.ReleaseComObject(_subiekt);
//                _subiekt = null;
//            }
//            GC.Collect();
//            GC.WaitForPendingFinalizers();
//        }
//    }
//}