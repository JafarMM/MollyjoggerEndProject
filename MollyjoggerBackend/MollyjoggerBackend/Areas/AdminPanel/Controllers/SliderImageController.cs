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

     
            var fileName = $"{Guid.NewGuid()}-{sliderImages.Photo.FileName}";
            var filePath = Path.Combine(_environment.WebRootPath,"Images",fileName);

            using(var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await sliderImages.Photo.CopyToAsync(fileStream);
            }

            sliderImages.Image = fileName;
            await _dbContext.SliderImages.AddAsync(sliderImages);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }
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

            var fileName = $"{Guid.NewGuid()}-{sliderImages.Photo.FileName}";
            var filePath = Path.Combine(_environment.WebRootPath, "Images", fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await sliderImages.Photo.CopyToAsync(fileStream);
            }

            sliderImages.Image = fileName;
            _dbContext.SliderImages.Remove(sliderImages);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
