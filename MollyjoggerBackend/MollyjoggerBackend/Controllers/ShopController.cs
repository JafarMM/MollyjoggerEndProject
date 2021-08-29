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

        #region ShopIndex
        public IActionResult Index()
        {
          
            var products = _dbContext.ShopOfProducts.Where(x=> x.IsDeleted==false).Include(x=>x.ProductDetails).Include(x=> x.ProductCategories).ThenInclude(x=> x.Category).OrderByDescending(x => x.Id).Take(6).ToList();
            return View(products);
        }
        #endregion

        #region LoadMore
        public IActionResult Load(int skip, string id)
        {
            if (skip >= _productsCount)
            {
                return Content("Error");
            }

            var products = new List<ShopOfProducts>();

            if (id == "0")
            {
                products = _dbContext.ShopOfProducts.OrderByDescending(x => x.Id).Skip(skip).Take(6).ToList();
            }
            else
            {
                products = _dbContext.ShopOfProducts.Include(x => x.ProductCategories)
                .Where(x => x.ProductCategories.Any(x => x.CategoryId == Convert.ToInt32(id)))
                .OrderByDescending(x => x.Id).Skip(skip).Take(6).ToList();
            }

            return PartialView("_ProductPartial", products);

          
        }
        #endregion

        #region ShopDetails
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
        #endregion

        #region FilterShopCategory
        public async Task<IActionResult> Filter(string id)
        {
            var products = new List<ShopOfProducts>();
            if(id == "0")
            {
                products = await _dbContext.ShopOfProducts.Where(x => x.IsDeleted == false).ToListAsync();
            }
            else
            {
                products = await _dbContext.ShopOfProducts.Include(x => x.ProductCategories)
                .Where(x => x.ProductCategories.Any(x => x.CategoryId == Convert.ToInt32(id)) && x.IsDeleted == false).ToListAsync();
            }

            return PartialView("_ProductPartial",products);
        }
        #endregion
    }
}
