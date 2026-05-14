using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using CollectorApp.Api.Data;
using CollectorApp.Api.Dtos;
using CollectorApp.Api.Exceptions;
using CollectorApp.Api.Interfaces;
using CollectorApp.Api.Models;
using Serilog;

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
            Log.Information("Dodawanie nowego kodu kreskowego: {Barcode}", request.Value);
            try
            {
                var barcode = _mapper.Map<Barcode>(request);
                barcode.CreatedAt = DateTime.UtcNow;
                _context.Barcodes.Add(barcode);
                await _context.SaveChangesAsync();
                Log.Debug("Kod kreskowy zapisany pomyślnie w SQLite.");
                return _mapper.Map<BarcodeResponseDto>(barcode);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Błąd podczas zapisu kodu kreskowego do bazy.");
                throw;
            }
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