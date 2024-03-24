using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KnowledgeNexus.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace KnowledgeNexus
{
    public class Program
    {


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<KnowledgeNexusContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("KnowledgeNexusContext") ?? throw new InvalidOperationException("Connection string 'KnowledgeNexusContext' not found.")));

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                    options.SlidingExpiration = true; //resetting the clock
                    options.LoginPath = "/BookAdmin/Login"; 
                    options.LogoutPath = "/BookAdmin/Logout";
                    options.AccessDeniedPath = "/AccessDenied/";
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //both UseAuthentication and UseAuthorization must be applied after UseRouting

            app.UseAuthentication(); //<---Added UserAuthentication
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
