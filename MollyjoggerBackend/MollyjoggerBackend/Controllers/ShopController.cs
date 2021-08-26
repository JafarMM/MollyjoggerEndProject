using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MollyjoggerBackend.DataAccesLayer;
using MollyjoggerBackend.Models;
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

        public IActionResult Index(int? categoryId,int page=1)
        {
            var products = new List<ShopOfProducts>();

            if (categoryId == null)
            {
                ViewBag.PageCount = Decimal.Ceiling((decimal)_dbContext.ShopOfProducts.Where(x => x.IsDeleted == false).Take(6).Count() / 9);
                ViewBag.Page = page;
                if (ViewBag.PageCount < page || page <= 0)
                    return NotFound();

                return View(products);
            }
            else
            {
                var productCategory = _dbContext.productCategories.Where(x => x.CategoryId == categoryId)
                    .Include(x => x.ShopOfProducts).Where(x => x.ShopOfProducts.IsDeleted == false).OrderByDescending(x => x.ShopOfProducts.LastModificationTime);

                if (productCategory.Count() == 0)
                    return NotFound();
                foreach (var productcategory in productCategory)
                {
                    products.Add(productcategory.ShopOfProducts);
                }
                return View(products);
            }
            //var products = _dbContext.ShopOfProducts.Where(x=> x.IsDeleted==false).Include(x=>x.ProductDetails).Include(x=> x.ProductCategories).ThenInclude(x=> x.Category).OrderByDescending(x => x.Id).Take(6).ToList();
            
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
