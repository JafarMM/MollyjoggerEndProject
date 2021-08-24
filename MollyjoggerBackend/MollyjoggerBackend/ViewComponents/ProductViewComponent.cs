using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MollyjoggerBackend.DataAccesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.ViewComponents
{
    public class ProductViewComponent:ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public ProductViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _dbContext.ShopOfProducts.ToListAsync();
            return View(products);
        }
    }
}
