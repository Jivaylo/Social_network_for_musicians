using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data.SeedData;
using System;
using SocialNetworkMusician.Services;
using SocialNetworkMusician.Services.Implementations;
using SocialNetworkMusician.Services.Interfaces;

namespace SocialNetworkMusician
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddTransient<IEmailSender, FakeEmailSender>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IErrorService, ErrorService>();
            builder.Services.AddScoped<IMessagesService, MessagesService>();
            builder.Services.AddScoped<IPlaylistsService, PlaylistsService>();
            builder.Services.AddScoped<IReportsService, ReportsService>();
            builder.Services.AddScoped<ITracksService, TracksService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddRazorPages();
           
            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error/ErrorPage");
                app.UseStatusCodePagesWithReExecute("/Error/ErrorPage");
            }
            // await app.SeedAsync();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapFallbackToController("ErrorPage", "Error");
            app.MapRazorPages();

            app.Run();
        }
        public class FakeEmailSender : IEmailSender
        {
            public Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=========================================");
                Console.WriteLine($"?? EMAIL SENT TO: {email}");
                Console.WriteLine($"Subject: {subject}");
                Console.WriteLine($"Body: {htmlMessage}");
                Console.WriteLine("=========================================");
                Console.ResetColor();
                return Task.CompletedTask;
            }
        }
    }
}