using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;

namespace WebFrontToBack.ViewComponents
{
    public class SliderViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        public SliderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.Sliders.ToListAsync());
        }
    }
}
