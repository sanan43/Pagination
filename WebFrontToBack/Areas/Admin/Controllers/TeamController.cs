using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.Areas.Admin.ViewModels;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;
using WebFrontToBack.Utilities.Extensions;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TeamController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }
        public async Task<IActionResult> Index()
        {
            ICollection<TeamMember> teamMembers = await _context.TeamMembers.ToListAsync();
            return View(teamMembers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamMemberVM teamMembers)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!teamMembers.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{teamMembers.Photo.FileName} type must be image type");
                return View();
            }
            if (!teamMembers.Photo.CheckFileSize(400))
            {
                ModelState.AddModelError("Photo", $"{teamMembers.Photo.FileName} file type must be size less than 200kb");
                return View();
            }
            string root = Path.Combine
                (_webHostEnvironment.WebRootPath, "assets", "img");

            string fileName = await teamMembers.Photo.SaveAsync(root);



            TeamMember teamMember = new TeamMember()
            {
                FullName = teamMembers.FullName,
                ImagePath = fileName,
                Profession = teamMembers.Profession
            };
            await _context.TeamMembers.AddAsync(teamMember);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int Id)
        {
            TeamMember? teamMember = _context.TeamMembers.Find(Id);

            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        [HttpPost]
        public IActionResult Update(TeamMember teamMember)
        {
            TeamMember? editedTeamMember = _context.TeamMembers.Find(teamMember.Id);
            if (editedTeamMember == null)
            {
                return NotFound();
            }
            editedTeamMember.FullName = teamMember.FullName;
            _context.TeamMembers.Update(editedTeamMember);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            TeamMember? teamMember = _context.TeamMembers.Find(Id);
            if (teamMember == null)
            {
                return NotFound();
            }
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", teamMember.ImagePath);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Exists(imagePath);
            }
            _context.TeamMembers.Remove(teamMember);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

