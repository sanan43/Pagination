using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.Areas.Admin.ViewModels;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;
using WebFrontToBack.Utilities.Extensions;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RecentWorksController : Controller
    {
        private readonly AppDbContext _Context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecentWorksController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _Context = context;
            _webHostEnvironment = webHostEnvironment;

        }
        public async Task<IActionResult> Index()
        {
            ICollection<RecentWorks> recentWorks =await _Context.RecentWorks.ToListAsync();
            return View(recentWorks);
        }
        [HttpGet]
         public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecentWorksVM recentWorks)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!recentWorks.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{recentWorks.Photo.FileName} type must be image type");
                return View();
            }
            if (!recentWorks.Photo.CheckFileSize(400))
            {
                ModelState.AddModelError("Photo", $"{recentWorks.Photo.FileName} file type must be size less than 200kb");
                return View();
            }
            string root = Path.Combine
                (_webHostEnvironment.WebRootPath, "assets", "img");

            string fileName = await recentWorks.Photo.SaveAsync(root);



            RecentWorks recentWorks1 = new RecentWorks()
            {
                Name = recentWorks.Name,
                ImagePath = fileName,
                Description = recentWorks.Description
            };
            await _Context.RecentWorks.AddAsync(recentWorks1);  
            await _Context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int Id)
        {
            RecentWorks? recentWorks = _Context.RecentWorks.Find(Id);
            if (recentWorks == null)
            {
                return NotFound();
            }
            return View(recentWorks);
        }
        [HttpPost]
        public IActionResult Update(RecentWorks recentWorks)
        {
            RecentWorks? editedRecentWorks = _Context.RecentWorks.Find(recentWorks.Id);
            if (editedRecentWorks == null)
            {
                return NotFound();
            }
            editedRecentWorks.Name = recentWorks.Name;
            _Context.RecentWorks.Update(editedRecentWorks);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            RecentWorks? recentWorks = _Context.RecentWorks.Find(id);
            if (recentWorks == null)
            {
                return NotFound();
            }
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", recentWorks.ImagePath);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Exists(imagePath);
            }
            _Context.RecentWorks.Remove(recentWorks);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
