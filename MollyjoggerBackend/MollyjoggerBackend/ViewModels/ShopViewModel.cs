using MollyjoggerBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.ViewModels
{
    public class ShopViewModel
    {
        public List<ShopOfProducts> shopOfProducts { get; set; }
        public List<Category> Categories { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ProductDetails ProductDetails { get; set; }
    }
}
