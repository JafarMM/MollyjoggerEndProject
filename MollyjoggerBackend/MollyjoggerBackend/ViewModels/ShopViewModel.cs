using MollyjoggerBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.ViewModels
{
    public class ShopViewModel
    {
       public ShopOfProducts ShopOfProducts { get; set; }
       public List<Category> Categories { get; set; }
    }
}
