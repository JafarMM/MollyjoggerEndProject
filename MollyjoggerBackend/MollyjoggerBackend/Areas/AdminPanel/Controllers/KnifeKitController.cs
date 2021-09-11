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
    public class KnifeKitController:Controller
    {
        private readonly AppDbContext _dbContext;

        public KnifeKitController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var knifekit = _dbContext.KnifeKit.FirstOrDefault();
            return View(knifekit);
        }
        #region KnifeKitUpdate
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();
            var knifekit = await _dbContext.KnifeKit.SingleOrDefaultAsync(x => x.Id == id);
            if (knifekit == null)
            {
                return NotFound();
            }
            return View(knifekit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, KnifeKit knifekit)
        {
            if (id == null)
                return NotFound();

            if (id != knifekit.Id)
                return BadRequest();

            var knifekit1 = await _dbContext.KnifeKit.SingleOrDefaultAsync(x => x.Id == id);
            if (knifekit1 == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var fileName = knifekit1.Image;

            if (knifekit.Photo != null)
            {
                if (!knifekit.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Yuklediyiniz shekil deyildir!!!");
                    return View();
                }

                if (!knifekit.Photo.IsSizeAllowed(4000))
                {
                    ModelState.AddModelError("Photo", "Yuklediyiniz sheklin olchusu 4 mb dan az olmalidir!!!.");
                    return View();
                }

                var path = Path.Combine(Constants.ImageFolderPath, "knifekit", knifekit1.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                fileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, knifekit.Photo);
            }

            knifekit1.Image = fileName;
            knifekit1.Title = knifekit.Title;
            knifekit1.Description = knifekit.Description;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
    }
}
