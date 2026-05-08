using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CollectorApp.Api.Dtos;

namespace CollectorApp.Api.Interfaces
{
    public interface IBarcodeService
    {
        Task<IEnumerable<BarcodeResponseDto>> GetAllAsync();
        Task<BarcodeResponseDto> GetByIdAsync(int id);
        Task<BarcodeResponseDto> CreateAsync(BarcodeCreateDto request);
        //Task<BarcodeResponseDto> UpdateAsync(int id, BarcodeCreateDto request);
        Task DeleteAsync(int id);
    }
}