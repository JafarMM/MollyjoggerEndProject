using Microsoft.EntityFrameworkCore;
using MollyjoggerBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.DataAccesLayer
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {

        }
        public DbSet<SliderImages> SliderImages { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Shoppartreklam> Shoppartreklams { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Bio> Bio { get; set; }
        public DbSet<ShopOfProducts> ShopOfProducts { get; set; }
    }
}
