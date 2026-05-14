using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using CollectorApp.Api.Dtos.SubiektDtos;
using InsERT;
using Serilog;

namespace CollectorApp.Api.Services
{
    public class SubiektGTService : IDisposable
    {
        public T Execute<T>(Func<Subiekt, T> action)
        {
            Log.Information("Rozpoczynanie operacji w Subiekt GT...");
            var sw = Stopwatch.StartNew();
            T result = default(T);
            Exception exception = null;

            var thread = new Thread(() =>
            {
                GT gt = null;
                Subiekt subiekt = null;

                try
                {
                    gt = new GT();
                    gt.Produkt = ProduktEnum.gtaProduktSubiekt;
                    gt.Serwer = ConfigurationManager.AppSettings["SubiektServer"];
                    gt.Baza = ConfigurationManager.AppSettings["SubiektDatabase"];
                    gt.Autentykacja = AutentykacjaEnum.gtaAutentykacjaMieszana;
                    gt.Uzytkownik = ConfigurationManager.AppSettings["SubiektUser"];
                    gt.UzytkownikHaslo = ConfigurationManager.AppSettings["SubiektPass"];
                    gt.Operator = ConfigurationManager.AppSettings["SubiektOperator"];
                    gt.OperatorHaslo = ConfigurationManager.AppSettings["SubiektOperatorPass"];

                    Log.Debug("Inicjalizacja Sfery dla operatora: {Operator}", gt.Operator);
                    subiekt = (Subiekt)gt.Uruchom(1, 4);

                    if (subiekt == null)
                    {
                        Log.Error("Nie udało się uruchomić Sfery Subiekta GT.");
                        throw new Exception("Błąd inicjalizacji Sfery. Sprawdź parametry logowania.");
                    }
                    sw.Stop();
                    Log.Information("Operacja zakończona sukcesem w {Elapsed}ms", sw.ElapsedMilliseconds);
                    result = action(subiekt);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Wystąpił krytyczny błąd podczas komunikacji z Subiekt GT");
                    exception = ex;
                }
                finally
                {
                    if (subiekt != null)
                    {
                        Marshal.ReleaseComObject(subiekt);
                    }

                    if (gt != null)
                    {
                        Marshal.ReleaseComObject(gt);
                    }
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (exception != null)
            {
                throw exception;
            }
            return result;
        }

        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public List<WarehouseDto> GetWarehouses()
        {
            var warehouses = new List<WarehouseDto>();

            using (var connection = new SqlConnection(BuildSubiektConnectionString()))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            mag_Id,
                            mag_Symbol,
                            mag_Nazwa,
                            mag_Opis
                        FROM dbo.sl_Magazyn
                        ORDER BY mag_Nazwa, mag_Symbol";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            warehouses.Add(new WarehouseDto
                            {
                                Id = reader.GetInt32(0),
                                Symbol = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Name = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Description = reader.IsDBNull(3) ? null : reader.GetString(3)
                            });
                        }
                    }
                }
            }
            Log.Information("Pobrano {Count} magazynów z bazy Subiekt GT", warehouses.Count);
            return warehouses;
        }

        public List<CustomerDto> GetCustomers()
        {
            return Execute(subiekt =>
            {
                var customers = new List<CustomerDto>();

                foreach (Kontrahent kh in subiekt.Kontrahenci)
                {
                    customers.Add(new CustomerDto
                    {
                        Id = (int)kh.Identyfikator,
                        Symbol = kh.Symbol?.ToString(),
                        Name = kh.Nazwa?.ToString(),
                        TaxId = kh.NIP?.ToString(),
                        IsActive = Convert.ToBoolean(kh.Aktywny)
                    });

                    Marshal.ReleaseComObject(kh);
                }
                Log.Information("Pobrano {Count} klientów z Subiekt GT", customers.Count);
                return customers;
            });
        }

        public List<ProductDto> GetProducts(int warehouseId)
        {
            return Execute(subiekt =>
            {
                var products = new List<ProductDto>();


                foreach (Towar tw in subiekt.Towary)
                {
                    //double currentStock = (double)tw.StanMaks(warehouseId);
                    
                    products.Add(new ProductDto
                    {
                        Id = (int)tw.Identyfikator,
                        Symbol = tw.Symbol?.ToString(),
                        Name = tw.Nazwa?.ToString(),
                        Barcode = tw.KodyKreskowe?.Podstawowy?.ToString(),
                        Unit = tw.Miary?.Podstawowa?.ToString(),
                        //Price = Convert.ToDecimal(tw.Ceny.Liczba),
                        //Stock = currentStock
                    });

                    Marshal.ReleaseComObject(tw);

                    if (products.Count >= 100) break;
                }
                Log.Information("Pobrano {Count} produktów z Subiekt GT dla magazynu ID: {WarehouseId}", products.Count, warehouseId);
                return products;
            });
        }

        public List<string> GetSubiektMembers()
        {
            return Execute(obj =>
            {
                var members = new List<string>();
                var type = obj.GetType();


                foreach (var m in type.GetMembers())
                {
                    members.Add($"{m.MemberType}: {m.Name}");
                }

                return members;
            });
        }

        private static string BuildSubiektConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = ConfigurationManager.AppSettings["SubiektServer"],
                InitialCatalog = ConfigurationManager.AppSettings["SubiektDatabase"],
                UserID = ConfigurationManager.AppSettings["SubiektUser"],
                Password = ConfigurationManager.AppSettings["SubiektPass"],
                IntegratedSecurity = false
            };

            return builder.ConnectionString;
        }

       
    }
}
