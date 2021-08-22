using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public int ShopOfProductsId { get; set; }
        public ShopOfProducts ShopOfProducts { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
