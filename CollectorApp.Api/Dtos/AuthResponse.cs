using System;

namespace CollectorApp.Api.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string UserName { get; set; }
    }
}