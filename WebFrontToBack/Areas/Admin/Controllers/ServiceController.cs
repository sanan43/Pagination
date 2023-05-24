using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebFrontToBack.Areas.Admin.ViewModels;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;
using WebFrontToBack.Utilities.Constants;
using WebFrontToBack.Utilities.Extensions;
using WebFrontToBack.ViewModel;

namespace WebFrontToBack.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin,User")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private List<Category> _categories;
        private readonly IWebHostEnvironment _environment;
        private string _errorMessages;

        public ServiceController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _categories = _context.Categories.ToList();
            _environment = environment;
        }
        public async Task<IActionResult> Index(int page=1,int take=8)
        {
            List<Service> services = await _context.Services
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.Id)
                .Skip((page - 1) * take)
                .Take(take)
                .Include(s => s.Category)
                .Include(s => s.ServiceImages)
                .ToListAsync();
            int pageCount = await GetPageCount(take);
            PaginateVM<Service> model1 = new PaginateVM<Service>
            {
                Data = services,
                CurrentPage = page,
                PageCount = pageCount,
                HasNext = page < pageCount,
                HasPreview = page > 1

            };

            return View(model1);

        }
        private async Task<int> GetPageCount(int take)
        {
            int servicesCount=await _context.Services.CountAsync();
            return (int)Math.Ceiling((decimal)servicesCount / take);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateServiceVM createServiceVM = new CreateServiceVM()
            {
                Categories = _categories
            };
            return View(createServiceVM);


            //Service service = new Service();
            //ServiceVM serviceVM = new ServiceVM()
            //{
            //    categories = await _context.Categories.ToListAsync(),
            //    services = service

            //};
            //return View(serviceVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServiceVM ServiceVM)
        {
            ServiceVM.Categories = _categories;
            if (!ModelState.IsValid)
            {
                return View(ServiceVM);
            }
            if (!CheckPhoto(ServiceVM.Photos))
            {
                ModelState.AddModelError("Photos", _errorMessages);
            }
            string rootPath = Path.Combine(_environment.WebRootPath, "assets", "img");
            List<ServiceImage> images = await CreateFilesAndGetServiceImages(ServiceVM.Photos, rootPath);
            Service service = new Service()
            {
                Name = ServiceVM.Name,
                CategoryId = ServiceVM.CategoryId,
                Description = ServiceVM.Description,
                Price = ServiceVM.Price,
                ServiceImages = images,
            };
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private async Task<List<ServiceImage>> CreateFilesAndGetServiceImages(List<IFormFile> photos, string rootPath)
        {
            List<ServiceImage> images = new List<ServiceImage>();

            foreach (var photo in photos)
            {

                string fileName = await photo.SaveAsync(rootPath);
                ServiceImage serviceImage = new ServiceImage()
                {
                    Path = fileName
                };
                if (!images.Any(i => i.IsActive))
                {
                    serviceImage.IsActive = true;
                }
                images.Add(serviceImage);
            }

            return images;
        }

        //public async Task<IActionResult> Create(ServiceVM service)
        //{
        //    service.services.IsDeleted = false;
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    await _context.Services.AddAsync(service.services); 
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}



        public IActionResult Update(int Id)
        {
            Service? service = _context.Services.Find(Id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        public IActionResult Update(Service service)
        {
            Service? editedService = _context.Services.Find(service.Id);
            if (editedService == null)
            {
                return NotFound();
            }
            editedService.Name = service.Name;
            _context.Services.Update(editedService);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            Service? service = _context.Services.Find(Id);
            if (service == null)
            {
                return NotFound();
            }
            _context.Services.Remove(service);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        private bool CheckPhoto(List<IFormFile> photos)
        {
            foreach (var photo in photos)
            {
                if (!photo.CheckContentType("image/"))
                {
                    _errorMessages = $"{photo.FileName} - {Messages.FileTypeMustBeImage}";
                    return false;
                }
                if (!photo.CheckFileSize(10))
                {
                    _errorMessages = $"{photo.FileName} - {Messages.FileSizeMustBe200KB}";
                    return false;
                }
            }
            return true;
        }

    }
    
}

