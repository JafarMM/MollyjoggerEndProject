using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly int _productsCount;
        public ShopController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _productsCount = _dbContext.ShopOfProducts.Count();
        }

        public IActionResult Index()
        {

            ViewBag.ProductCount = _productsCount;

            var products = _dbContext.ShopOfProducts.Where(x=> x.IsDeleted==false).Include(x=>x.ProductDetails).Include(x=> x.ProductCategories).ThenInclude(x=> x.Category).OrderByDescending(x => x.Id).Take(6).ToList();
            return View(products);
        }
        public IActionResult Load(int skip)
        {
            if (skip >= _productsCount)
            {
                return Content("Error");
            }

            var products = _dbContext.ShopOfProducts.OrderByDescending(x => x.Id).Skip(skip).Take(6).ToList();

            return PartialView("_ProductPartial", products);

          
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productDetails = _dbContext.ProductDetails.Include(x => x.ShopOfProducts).FirstOrDefault(x => x.ShopOfProductsId == id);
            if (productDetails == null)
            {
                return NotFound();
            }
            return View(productDetails);
        }
    }
}
