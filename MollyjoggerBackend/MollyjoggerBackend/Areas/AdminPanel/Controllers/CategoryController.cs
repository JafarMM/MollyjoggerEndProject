using Microsoft.AspNetCore.Mvc;
using MollyjoggerBackend.DataAccesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var categories = _dbContext.Categories.ToList();
            return View(categories);
        }
    }
}
