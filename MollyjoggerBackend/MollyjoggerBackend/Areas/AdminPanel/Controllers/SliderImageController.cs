using Microsoft.AspNetCore.Hosting;
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
    public class SliderImageController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;
 
        public SliderImageController(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var sliderImages = _dbContext.SliderImages.ToList();
            return View(sliderImages);
        }
        #region SliderImageCreate
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderImages sliderImages)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!sliderImages.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "It is not a picture");
                return View();
            }
            if(!sliderImages.Photo.IsSizeAllowed(3072))
            {
                ModelState.AddModelError("Photo", "Your picture higher than 3 mb");
                    return View();
            }


            var fileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, sliderImages.Photo);
             

            sliderImages.Image = fileName;
            await _dbContext.SliderImages.AddAsync(sliderImages);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        #endregion

        #region SlideImageDelete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var sliderImages = await _dbContext.SliderImages.FindAsync(id);
            if (sliderImages == null)
                return NotFound();

            return View(sliderImages);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteSlider(int? id)
        {
            if (id == null)
                return NotFound();

            var sliderImages = await _dbContext.SliderImages.FindAsync(id);

            if (sliderImages == null)
                return NotFound();

            var path = Path.Combine(Constants.ImageFolderPath, sliderImages.Image);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _dbContext.SliderImages.Remove(sliderImages);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion

        #region SliderImageUpdate
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();

            var sliderImages = await _dbContext.SliderImages.FindAsync(id);
            if (sliderImages == null)
                return NotFound();

            return View(sliderImages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SliderImages sliderImages)
        {
            if (id == null)
                return NotFound();

            if (id != sliderImages.Id)
                return BadRequest();

            var SliderImages = await _dbContext.SliderImages.FindAsync(id);
            if (SliderImages == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var fileName = SliderImages.Image;

            if (sliderImages.Photo != null)
            {
                if (!sliderImages.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Yukledyiniz shekil deyildir!");
                    return View();
                }

                if (!sliderImages.Photo.IsSizeAllowed(4000))
                {
                    ModelState.AddModelError("Photo", "Yuklediyiniz sheklin olchusu 4 mb dan chox olmamalidir!");
                    return View();
                }

                var path = Path.Combine(Constants.ImageFolderPath, "sliderImages", SliderImages.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                fileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath,sliderImages.Photo);
            }

            SliderImages.Image = fileName;
    
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion

        #region SliderImageDetails
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var sliderImages = _dbContext.SliderImages.Find(id);

            if (sliderImages == null)
                return NotFound();

            return View(sliderImages);
        }
        #endregion
    }
}
