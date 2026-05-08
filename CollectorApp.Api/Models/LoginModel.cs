using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectorApp.Api.Models
{
    public class LoginModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}