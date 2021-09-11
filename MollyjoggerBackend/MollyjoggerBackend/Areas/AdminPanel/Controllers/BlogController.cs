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
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;

        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task <IActionResult> Index()
        {
            var blogs = await _dbContext.Blogs.Include(x=> x.BlogDetails).Where(x=> x.IsDeleted==false).OrderByDescending(x=> x.LastModificationTime).ToListAsync();
            return View(blogs);
        }
        #region CreateBlog
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (blog.Photo == null)
            {
                ModelState.AddModelError("Photo", "Shekil bosh olmamalidir!");
                return View();
            }
            if (!blog.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Yuklediyiniz shekil deyildir!!!");
                return View();
            }
            if (!blog.Photo.IsSizeAllowed(4000))
            {
                ModelState.AddModelError("Photo", "Yuklediyiniz sheklin olchusu 4 mb dan az olmalidir.");
                return View();
            }
            var fileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, blog.Photo);

            blog.Image = fileName;

            var isExist = await _dbContext.Blogs.AnyAsync(x => x.Title.ToLower() == blog.Title.ToLower() && x.IsDeleted == false);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda blog movcuddur");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            blog.LastModificationTime = DateTime.Now;

            await _dbContext.AddRangeAsync(blog, blog.BlogDetails);
            await _dbContext.SaveChangesAsync();

            List<Subscribe> subscribes = _dbContext.Subscribes.ToList();
            string subject = "Create Blog!";
            string url = "https://localhost:44368/Blog/Details/" + blog.Id;
            string message = $"<a href={url}>Hey!We have a new Blog!Click here and come on our Shop!</a>";
            foreach (Subscribe sub in subscribes)
            {
                await Helper.SendMessage(subject, message, sub.Email);
            }

            return RedirectToAction("Index");

        }
        #endregion
        #region UpdateBlog
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blog = await _dbContext.Blogs.Include(x => x.BlogDetails).Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Blog blog)
        {
            if (id == null)
                return NotFound();

            if (id != blog.Id)
                return NotFound();
            var blog1 = await _dbContext.Blogs.Include(x => x.BlogDetails).Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (blog1 == null)
            {
                return NotFound();
            }
            var fileName = blog1.Image;

            if (blog.Photo != null)
            {
                if (!blog.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Yuklediyiniz shekil deyildir!!!");
                    return View();
                }
                if (!blog.Photo.IsSizeAllowed(4000))
                {
                    ModelState.AddModelError("Photo", "Yuklediyiniz sheklin hecmi 4 mb dan az olmalidir!");
                    return View();
                }
                var path = Path.Combine(Constants.ImageFolderPath, "blog", blog1.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, blog.Photo);
            }

            var isExist = await _dbContext.Blogs.AnyAsync(x => x.Title == blog.Title && x.Id != blog.Id && x.IsDeleted == false);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda blog vardir");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            blog1.Image = fileName;
            blog1.Title = blog.Title;
            blog1.BlogDetails.Description = blog.BlogDetails.Description;
            blog1.BlogDetails = blog.BlogDetails;
            blog1.LastModificationTime = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
        #region DeleteBlog
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var blog = await _dbContext.Blogs.Include(x => x.BlogDetails)
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (blog == null)
                return NotFound();

            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteBlog(int? id)
        {
            if (id == null)
                return NotFound();

            var blog = await _dbContext.Blogs.Include(x => x.BlogDetails)
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (blog == null)
                return NotFound();

            blog.IsDeleted = true;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
        #region DetailBLog
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var blog = await _dbContext.BlogDetails.Include(x => x.Blog).FirstOrDefaultAsync(x => x.BlogId == id);

            if (blog == null)
                return NotFound();

            return View(blog);
        }
        #endregion
    }
}
