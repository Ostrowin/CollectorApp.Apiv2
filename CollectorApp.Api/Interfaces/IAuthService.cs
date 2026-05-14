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
        bool Authenticate(LoginModel model); 
        AuthResponseDto GenerateToken(string fullName);
    }
}