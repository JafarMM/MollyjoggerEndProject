using Microsoft.AspNetCore.Mvc;
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
    public class ShopPartReklamController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ShopPartReklamController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var shopreklam = _dbContext.Shoppartreklams.ToList();
            return View(shopreklam);
        }

        #region ShopPartReklamUpdate
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();

            var shopPart = await _dbContext.Shoppartreklams.FindAsync(id);
            if (shopPart == null)
                return NotFound();

            return View(shopPart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Shoppartreklam shopPart)
        {
            if (id == null)
                return NotFound();

            if (id != shopPart.Id)
                return BadRequest();

            var ShopPart = await _dbContext.Shoppartreklams.FindAsync(id);
            if (ShopPart == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var fileName = ShopPart.Image;

            if (ShopPart.Photo != null)
            {
                if (!ShopPart.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Yukledyiniz shekil deyildir!");
                    return View();
                }

                if (!ShopPart.Photo.IsSizeAllowed(4000))
                {
                    ModelState.AddModelError("Photo", "Yuklediyiniz sheklin olchusu 4 mb dan chox olmamalidir!");
                    return View();
                }

                var path = Path.Combine(Constants.ImageFolderPath, "sliderImages", ShopPart.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                fileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, ShopPart.Photo);
            }

            ShopPart.Image = fileName;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        #endregion

        #region ShopPartReklamDetails
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var shopPart = _dbContext.Shoppartreklams.Find(id);

            if (shopPart == null)
                return NotFound();

            return View(shopPart);
        }
        #endregion
    }
}
