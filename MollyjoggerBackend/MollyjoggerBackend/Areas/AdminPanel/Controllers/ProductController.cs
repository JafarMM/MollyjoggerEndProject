using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MollyjoggerBackend.Areas.AdminPanel.Utils;
using MollyjoggerBackend.DataAccesLayer;
using MollyjoggerBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task <IActionResult> Index()
        {
            ViewBag.PageCount = Decimal.Ceiling((decimal)_dbContext.ShopOfProducts.Where(x => x.IsDeleted == false).Count() / 5);
            

            var courses = await _dbContext.ShopOfProducts.Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.LastModificationTime).ToListAsync();

            return View(courses);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _dbContext.Categories.Where(x => x.IsDeleted == false).ToListAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShopOfProducts shopOfProducts, int?[] categoryId)
        {
            var categories = await _dbContext.Categories.Where(x => x.IsDeleted == false).ToListAsync();
            ViewBag.Categories = categories;

            foreach (var item in categoryId)
            {
                if (categories.All(x => x.Id != item))
                    return NotFound();
            }

            if (shopOfProducts.Photos1 == null)
            {
                ModelState.AddModelError("Photos1", "Photo field cannot be empty");
                return View();
            }

            if (!shopOfProducts.Photos1.IsImage())
            {
                ModelState.AddModelError("Photos1", "This is not a picture");
                return View();
            }

            if (!shopOfProducts.Photos1.IsSizeAllowed(3000))
            {
                ModelState.AddModelError("Photos1", "The size of the image you uploaded is 3 MB higher.");
                return View();
            }

            var fileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath , shopOfProducts.Photos1);

            shopOfProducts.Image1 = fileName;

            if (!ModelState.IsValid)
            {
                return View(shopOfProducts);
            }

           
            if (categoryId.Length == 0 || categoryId == null)
            {
                ModelState.AddModelError("", "Please select category.");
                return View(shopOfProducts);
            }

            foreach (var item in categoryId)
            {
                if (categories.All(x => x.Id != (int)item))
                    return BadRequest();
            }

            var productCategoryList = new List<ProductCategory>();
            foreach (var item in categoryId)
            {
                var categoryCourse = new ProductCategory
                {
                    CategoryId = (int)item,
                    ShopOfProductsId = shopOfProducts.Id
                };
                productCategoryList.Add(categoryCourse);
            }
            shopOfProducts.ProductCategories = productCategoryList;
 
            if (shopOfProducts.Price < 0)
            {
                ModelState.AddModelError("shopOfProducts.Price", "Price can not be negative.");
                return View(shopOfProducts);
            }

            shopOfProducts.CreationTime = DateTime.Now;
            shopOfProducts.LastModificationTime = DateTime.Now;

            await _dbContext.AddRangeAsync(shopOfProducts, shopOfProducts.ProductDetails);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
