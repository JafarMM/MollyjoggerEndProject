using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Models
{
    public class ShopOfProducts
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string ProductName { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastModificationTime { get; set; }
        public double Price { get; set; }        
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ProductDetails ProductDetails { get; set; }
        [NotMapped]
        public IFormFile Photos1 { get; set; }

        [NotMapped]
        public IFormFile Photos2 { get; set; }
    }
}
