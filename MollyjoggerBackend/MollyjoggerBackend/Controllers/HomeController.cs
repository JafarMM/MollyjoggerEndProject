using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MollyjoggerBackend.DataAccesLayer;
using MollyjoggerBackend.Models;
using MollyjoggerBackend.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var sliderimages = _dbContext.SliderImages.ToList();
            var products = _dbContext.Products.ToList();
            var shoppartreklam = _dbContext.Shoppartreklams.ToList();
            var aboutus = _dbContext.AboutUs.FirstOrDefault();
            var homeViewModel = new HomeViewModel
            {
                sliderImages = sliderimages,
                Products=products,
                Shoppartreklams=shoppartreklam,
                AboutUs=aboutus
            };
            return View(homeViewModel);
        }
        public IActionResult Search(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return Content("Error");
            }

            var products = _dbContext.ShopOfProducts.OrderByDescending(x => x.Id).Where(x => x.ProductName.Contains(search.ToLower())).Take(3).ToList();
            var searchViewModel = new SearchViewModel
            {
                ShopOfProducts = products
            };
            return PartialView("_SearchGlobalPartial", searchViewModel);
        }
       
    }
}
