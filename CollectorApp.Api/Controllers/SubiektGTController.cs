using System;
using System.Web.Http;
using CollectorApp.Api.Services;

namespace CollectorApp.Api.Controllers
{
    //[Authorize]
    [RoutePrefix("api/subiekt")]
    public class SubiektGTController : ApiController
    {
        private readonly SubiektGTService _subiektService;

        public SubiektGTController(SubiektGTService subiektService)
        {
            _subiektService = subiektService;
        }

        [HttpGet]
        [Route("test")]
        public IHttpActionResult TestConnection()
        {
            try
            {

                var result = _subiektService.GetSubiektMembers();
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("warehouses")]
        public IHttpActionResult GetWarehouses()
        {
            try
            {
                var result = _subiektService.GetWarehouses();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("customers")]
        public IHttpActionResult GetCustomers()
        {
            try
            {
                var result = _subiektService.GetCustomers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("products/{warehouseId:int}")]
        public IHttpActionResult GetProducts(int warehouseId)
        {
            try
            {
                var data = _subiektService.GetProducts(warehouseId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}