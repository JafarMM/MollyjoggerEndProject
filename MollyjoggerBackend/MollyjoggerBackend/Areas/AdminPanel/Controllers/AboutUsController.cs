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
    public class AboutUsController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AboutUsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var aboutus = _dbContext.AboutUs.FirstOrDefault();
            return View(aboutus);
        }

        #region AboutUsUpdate
        public async Task <IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();
            var aboutus =await  _dbContext.AboutUs.SingleOrDefaultAsync(x=> x.Id==id);
            if (aboutus == null)
            {
                return NotFound();
            }
            return View(aboutus);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, AboutUs about)
        {
            if (id == null)
                return NotFound();

            if (id != about.Id)
                return BadRequest();

            var about1 = await _dbContext.AboutUs.SingleOrDefaultAsync(x => x.Id == id);
            if (about1 == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var fileName = about1.Image;

            if (about.Photo != null)
            {
                if (!about.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Yuklediyiniz shekil deyildir!!!");
                    return View();
                }

                if (!about.Photo.IsSizeAllowed(4000))
                {
                    ModelState.AddModelError("Photo", "Yuklediyiniz sheklin olchusu 4 mb dan az olmalidir!!!.");
                    return View();
                }

                var path = Path.Combine(Constants.ImageFolderPath, "about", about1.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                fileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, about.Photo);
            }

            about1.Image = fileName;
            about1.Title = about.Title;
            about1.Description = about.Description;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
    }
}
