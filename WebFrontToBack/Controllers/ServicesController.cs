using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Controllers
{
    public class ServicesController : Controller
    {
        private readonly AppDbContext _context;

        public ServicesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.dbServiceCount=await _context.Services.CountAsync();

            return View(await _context.Services
                .Include(s => s.ServiceImages)
                .OrderByDescending(s => s.Id)
                .Take(8)
                .Where(s => !s.IsDeleted)
                .ToListAsync()
                );
        }
        public async Task<IActionResult> LoadMore(int skip=0)
        {
            List<Service> services = await _context.Services
                .Include(s => s.ServiceImages)
                .OrderByDescending(s => s.Id)
                .Where(s => !s.IsDeleted)
                .Skip(skip)
                .Take(8)
                .ToListAsync();
                
            return PartialView("_servicesPartialView", services);
        }
        public IActionResult GetSession()
        {
            string name = HttpContext.Session.GetString("Name");
            string surname = Request.Cookies["Surname"];
            return Json(name+" "+surname);
        }
    }
}
