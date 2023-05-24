using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebFrontToBack.DAL;
using WebFrontToBack.ViewModel;

namespace WebFrontToBack.Controllers
{
    public class BasketController : Controller
    {
        private const string COOKIES_BASKET = "basketVM";
        private readonly AppDbContext _appDbContext;
        public IActionResult Index()
        {
            List<BasketitemVM> basketitemVMs = new List<BasketitemVM>();
            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>
                (Request.Cookies[COOKIES_BASKET]);
            foreach (BasketVM item in basketVMs)
            {
                BasketitemVM basketitemVM = _appDbContext.Services
                    .Where(s => !s.IsDeleted && s.Id == item.ServiceId)
                    .Include(s => s.Category)
                    .Include(s => s.ServiceImages)
                    .Select(s => new BasketitemVM
                    {
                        Name = s.Name,
                        Id = s.Id,
                        CategoryName = s.Category.Name,
                        IsDeleted = s.IsDeleted,
                        Price = s.Price,
                        ServiceCount = item.Count,
                        ImagePath = s.ServiceImages.FirstOrDefault(i => i.IsActive).Path
                    }).FirstOrDefault();

                basketitemVMs.Add(basketitemVM);
                    
            }

            return View(basketitemVMs);
        }
        public IActionResult AddBasket(int id)
        {
            List<BasketVM> basketVMList;
            if (Request.Cookies[COOKIES_BASKET]!=null)
            {
                basketVMList = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[COOKIES_BASKET]);
            }
            else
            {
                basketVMList= new List<BasketVM> { };
            }

            BasketVM cookiesBasket =basketVMList.Where(s=>s.ServiceId == id).FirstOrDefault();
            if (cookiesBasket != null)
            {
                cookiesBasket.Count++;
            }
            else
            {
                BasketVM basketVM = new BasketVM() { ServiceId = id, Count = 1 };
                basketVMList.Add(basketVM);
            }
            
            
            Response.Cookies.Append(COOKIES_BASKET, JsonConvert.SerializeObject(basketVMList.OrderByDescending(s=>s.ServiceId)));
            return RedirectToAction("Index","Services");
        }
       
    }
}
