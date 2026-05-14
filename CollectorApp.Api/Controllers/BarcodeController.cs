using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CollectorApp.Api.Dtos;
using CollectorApp.Api.Interfaces;

namespace CollectorApp.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/barcodes")]
    public class BarcodesController : ApiController
    {
        private readonly IBarcodeService _barcodeService;

        public BarcodesController(IBarcodeService barcodeService)
        {
            _barcodeService = barcodeService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetBarcodes()
        {
            var result = await _barcodeService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetBarcode(int id)
        {
            var result = await _barcodeService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> PostBarcode(BarcodeCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _barcodeService.CreateAsync(dto);
            return CreatedAtRoute("DefaultApi", new { id = result.Id }, result);
        }
    }
}