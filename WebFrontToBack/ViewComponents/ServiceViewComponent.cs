using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.ViewComponents
{
    public class ServiceViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        public ServiceViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Service> services = await _context.Services
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.Id)
                .Take(8)
                .Include(s=>s.Category)
                .Include(s=>s.ServiceImages)
                .ToListAsync();
            
            return View(services);
        }
    }
}
