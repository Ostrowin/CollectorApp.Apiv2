using System;
using System.Data.SqlClient;
using CollectorApp.Api.Services;
using Serilog;

namespace CollectorApp.Api.Infrastructure
{
    public class HealthStatus
    {
        public bool IsDatabaseHealthy { get; set; }
        public bool IsSubiektHealthy { get; set; }
        public string Message { get; set; }
    }

    public class HealthCheckService
    {
        private readonly SubiektGTService _subiektService;

        public HealthCheckService(SubiektGTService subiektService)
        {
            _subiektService = subiektService;
        }

        public HealthStatus CheckEverything()
        {
            var status = new HealthStatus();

            try
            {
                status.IsDatabaseHealthy = true;
                Log.Debug("HealthCheck: Baza danych SQL działa.");
            }
            catch (Exception ex)
            {
                status.IsDatabaseHealthy = false;
                Log.Error(ex, "HealthCheck: Baza danych SQL nie odpowiada!");
            }

            try
            {
                var warehouses = _subiektService.GetWarehouses();
                status.IsSubiektHealthy = warehouses != null;
                Log.Debug("HealthCheck: Sfera GT działa poprawnie.");
            }
            catch (Exception ex)
            {
                status.IsSubiektHealthy = false;
                Log.Warning(ex, "HealthCheck: Sfera GT ma problemy!");
            }

            status.Message = (status.IsDatabaseHealthy && status.IsSubiektHealthy)
                ? "Wszystkie systemy sprawne."
                : "Wykryto problemy z usługami.";

            return status;
        }
    }
}