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
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;

        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Index
        public IActionResult Index()
        {
            var blog = _dbContext.Blogs.Where(x => x.IsDeleted == false).ToList();
           
            return View(blog);
        }
        #endregion
        #region Details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blogdetails=_dbContext.BlogDetails.Include(x=>x.Blog).FirstOrDefault(x => x.BlogId == id);
            return View(blogdetails);
        }
        #endregion
    }
}
