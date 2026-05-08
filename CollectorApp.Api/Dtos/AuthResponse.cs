using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectorApp.Api.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}