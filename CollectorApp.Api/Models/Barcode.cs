using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CollectorApp.Api.Enums;

namespace CollectorApp.Api.Models
{
    public class Barcode
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public BarcodeFormat Format { get; set; }
        public DateTime CreatedAt { get; set; }

        public Barcode()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}