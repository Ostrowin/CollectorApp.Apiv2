using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CollectorApp.Api.Models;

namespace CollectorApp.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DefaultConnection")
        { }

        public DbSet<Barcode> Barcodes { get; set; }
    }
}