using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MollyjoggerBackend.Areas.AdminPanel.Utils;
using MollyjoggerBackend.DataAccesLayer;
using MollyjoggerBackend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BloginhomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public BloginhomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region IndexBloginHome
        public IActionResult Index()
        {
            var bloginhome = _dbContext.Bloginhome.FirstOrDefault();
            return View(bloginhome);
        }
        #endregion
        #region BloginHomeUpdate
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();
            var bloginhome = await _dbContext.Bloginhome.SingleOrDefaultAsync(x => x.Id == id);
            if (bloginhome == null)
            {
                return NotFound();
            }
            return View(bloginhome);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Bloginhome bloginhome)
        {
            if (id == null)
                return NotFound();

            if (id != bloginhome.Id)
                return BadRequest();

            var bloginhome1 = await _dbContext.Bloginhome.SingleOrDefaultAsync(x => x.Id == id);
            if (bloginhome1 == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var fileName = bloginhome1.Image;

            if (bloginhome.Photo != null)
            {
                if (!bloginhome.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Yuklediyiniz shekil deyildir!!!");
                    return View();
                }

                if (!bloginhome.Photo.IsSizeAllowed(4000))
                {
                    ModelState.AddModelError("Photo", "Yuklediyiniz sheklin olchusu 4 mb dan az olmalidir!!!.");
                    return View();
                }

                var path = Path.Combine(Constants.ImageFolderPath, "knifekit", bloginhome1.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                fileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, bloginhome.Photo);
            }

            bloginhome1.Image = fileName;
            bloginhome1.Title = bloginhome.Title;
            bloginhome1.Description = bloginhome.Description;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
    }
}
