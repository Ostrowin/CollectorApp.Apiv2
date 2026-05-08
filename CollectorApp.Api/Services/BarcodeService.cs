using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using CollectorApp.Api.Data;
using CollectorApp.Api.Dtos;
using CollectorApp.Api.Interfaces;
using CollectorApp.Api.Models;
using System.Data.Entity;
using CollectorApp.Api.Exceptions;

namespace CollectorApp.Api.Services
{
    public class BarcodeService : IBarcodeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BarcodeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<BarcodeResponseDto> CreateAsync(BarcodeCreateDto request)
        {
            var barcode = _mapper.Map<Barcode>(request);
            barcode.CreatedAt = DateTime.UtcNow;
            _context.Barcodes.Add(barcode);
            await _context.SaveChangesAsync();
            return _mapper.Map<BarcodeResponseDto>(barcode);
        }
        public async Task DeleteAsync(int id)
        {
            var barcode = await _context.Barcodes.FindAsync(id);
            if (barcode == null)
            {
                throw new NotFoundException("Barcode not found");
            }
            _context.Barcodes.Remove(barcode);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<BarcodeResponseDto>> GetAllAsync()
        {
            var barcodes = await _context.Barcodes.ToListAsync();
            return _mapper.Map<IEnumerable<BarcodeResponseDto>>(barcodes);
        }
        public async Task<BarcodeResponseDto> GetByIdAsync(int id)
        {
            var barcode = await _context.Barcodes.FindAsync(id);
            if (barcode == null)
            {
                throw new NotFoundException("Barcode not found");
            }
            return _mapper.Map<BarcodeResponseDto>(barcode);
        }
    }

}