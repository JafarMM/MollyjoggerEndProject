using Microsoft.AspNetCore.Mvc;
using MollyjoggerBackend.DataAccesLayer;
using MollyjoggerBackend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ShopController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var shopofproducts = _dbContext.ShopOfProducts.ToList();
            var shopViewModel = new ShopViewModel
            {
                shopOfProducts=shopofproducts
            };
            return View(shopViewModel);
        }
    }
}
