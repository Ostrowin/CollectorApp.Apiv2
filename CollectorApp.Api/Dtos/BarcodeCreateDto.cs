using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CollectorApp.Api.Enums;

namespace CollectorApp.Api.Dtos
{
    public class BarcodeCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Value must be between 3 and 100 characters.")]
        public string Value { get; set; } = string.Empty;

        [Required(ErrorMessage = "Format is required")]
        public BarcodeFormat Format { get; set; }
    }
}