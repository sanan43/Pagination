using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.ViewComponents
{
    public class RecentWorkViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        public RecentWorkViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<RecentWorks> RecentWorks = await _context.RecentWorks
                
                .OrderByDescending(s => s.Id)
                .ToListAsync();

            return View(RecentWorks);
        }
    }
}
