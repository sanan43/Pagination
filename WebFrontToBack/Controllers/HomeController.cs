using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Migrations;
using WebFrontToBack.Models;
using WebFrontToBack.ViewModel;

namespace WebFrontToBack.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;
        
        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
       
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("Name", "Ali");
            Response.Cookies.Append("Surname", "Seferli",new CookieOptions { MaxAge=TimeSpan.FromSeconds(15)});
            HomeVM homeVM = new HomeVM()
            {
                RecentWorks = await _appDbContext.RecentWorks.ToListAsync(),
                
                Categories = await _appDbContext.Categories.Where(c => !c.IsDeleted).ToListAsync(),
                
            };
            
            return View(homeVM);
        }
        
    }
}
