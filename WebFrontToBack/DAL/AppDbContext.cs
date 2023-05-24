using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.Models;
using WebFrontToBack.Models.Auth;

namespace WebFrontToBack.DAL;

public class AppDbContext:IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {     
        
    }
    public DbSet<RecentWorks> RecentWorks { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceImage> ServiceImages { get; set; }
    public DbSet<WorkCategory> WorkCategories { get; set; }
    public DbSet<WorkService> WorkServices { get; set; }
    public DbSet<WorkServiceImage> WorkServiceImages { get; set; }
}
