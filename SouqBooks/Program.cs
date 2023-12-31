using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Models;
using SouqBooks.DataAccess.Data;
using SouqBooks.Utilities;
using Stripe;
using Utilities;

namespace SouqBooks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            //automatic binding stripe values to Stripe setting proprties 
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 5;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(cke => { 
                cke.LoginPath = "/Customer/Account/LoginPath";
                cke.AccessDeniedPath = "/Customer/Account/AccessDeniedPath";
            });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, cke =>
                    {
                       
                    });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
			builder.Services.AddScoped<ImageUploader,ImageUploader>(); // Register ImageUploader as a scoped service
            builder.Services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
            builder.Services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();     
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
           
            app.MapControllerRoute(
                 name: "Customer",
                 pattern: "Customer/{controller=Account}/{action=Login}"
                 );


            app.Run();
        }
    }
}