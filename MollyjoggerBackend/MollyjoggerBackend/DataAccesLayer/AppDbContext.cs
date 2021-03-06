using Microsoft.EntityFrameworkCore;
using MollyjoggerBackend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.DataAccesLayer
{
    public class AppDbContext: IdentityDbContext<User>
    {
        //Models included in the database
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {

        }
        public DbSet<SliderImages> SliderImages { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Shoppartreklam> Shoppartreklams { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Bio> Bio { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<ShopOfProducts> ShopOfProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductDetails> ProductDetails { get; set; }
        public DbSet<KnifeKit> KnifeKit { get; set; }
        public DbSet<ReturnForm> ReturnForm { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogDetails> BlogDetails { get; set; }
        public DbSet<Bloginhome> Bloginhome { get; set; }
        public DbSet<ProductCategory> productCategories { get; set; }

    }
}
