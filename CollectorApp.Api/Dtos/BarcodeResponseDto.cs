using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CollectorApp.Api.Enums;

namespace CollectorApp.Api.Dtos
{
    public class BarcodeResponseDto
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public BarcodeFormat Format { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}