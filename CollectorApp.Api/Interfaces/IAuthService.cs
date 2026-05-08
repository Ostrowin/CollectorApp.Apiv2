using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CollectorApp.Api.Dtos;
using CollectorApp.Api.Models;

namespace CollectorApp.Api.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(LoginModel model);
        Task<AuthResponseDto> LoginAsync(LoginModel model);
    }
}