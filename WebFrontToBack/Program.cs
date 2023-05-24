using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models.Auth;

namespace WebFrontToBack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromSeconds(10);

            });
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;

                opt.User.RequireUniqueEmail = true;

                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                opt.Lockout.AllowedForNewUsers = true;
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
                options.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);

            });
            var app = builder.Build();
            //if (!app.Environment.IsDevelopment())
            //{
            //    app.UseExceptionHandler("Home/Error");
            //}
           app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
   
                endpoints.MapDefaultControllerRoute();
            });

            app.Run();
        }
    }
}