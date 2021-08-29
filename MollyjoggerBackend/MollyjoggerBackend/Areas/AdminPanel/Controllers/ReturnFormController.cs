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
    public class ReturnFormController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ReturnFormController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var returnform = _dbContext.ReturnForm.FirstOrDefault();
            return View(returnform);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();
            var returnform = await _dbContext.ReturnForm.SingleOrDefaultAsync(x => x.id == id);
            if (returnform == null)
            {
                return NotFound();
            }
            return View(returnform);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, ReturnForm returnForm)
        {
            if (id == null)
                return NotFound();

            if (id != returnForm.id)
                return BadRequest();

            var returnform = await _dbContext.ReturnForm.SingleOrDefaultAsync(x => x.id == id);
            if (returnform == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var fileName = returnform.Image;

            if (returnForm.Photo != null)
            {
                if (!returnForm.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Yuklediyiniz shekil deyildir!!!");
                    return View();
                }

                if (!returnForm.Photo.IsSizeAllowed(4000))
                {
                    ModelState.AddModelError("Photo", "Yuklediyiniz sheklin olchusu 4 mb dan az olmalidir!!!.");
                    return View();
                }

                var path = Path.Combine(Constants.ImageFolderPath, "returnForm", returnform.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                fileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, returnForm.Photo);
            }

            returnform.Image = fileName;
            returnform.Title = returnForm.Title;
            returnform.Description = returnForm.Description;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
