using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialNetworkMusician.Data.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.SeedData
{
    public static class SeedData
    {
        private const string adminEmail = "admin@music.com";
        private const string adminPassword = "Admin123!";
        private const string demoEmail = "user@music.com";
        private const string demoPassword = "User123!";

        public static async Task SeedAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            string[] roles = { "Admin", "User", "Moderator" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // --- Admin ---
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    DisplayName = "Admin",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                    throw new Exception("Failed to create admin user.");
            }
            else
            {
                adminUser.EmailConfirmed = true;
                adminUser.LockoutEnabled = false;
                await userManager.UpdateAsync(adminUser);
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                await userManager.AddToRoleAsync(adminUser, "Admin");

            // --- Demo User ---
            var demoUser = await userManager.FindByEmailAsync(demoEmail);
            if (demoUser == null)
            {
                demoUser = new ApplicationUser
                {
                    UserName = demoEmail,
                    Email = demoEmail,
                    DisplayName = "Demo User",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                var result = await userManager.CreateAsync(demoUser, demoPassword);
                if (!result.Succeeded)
                    throw new Exception("Failed to create demo user.");
            }
            else
            {
                demoUser.EmailConfirmed = true;
                demoUser.LockoutEnabled = false;
                await userManager.UpdateAsync(demoUser);
            }

            if (!await userManager.IsInRoleAsync(demoUser, "User"))
                await userManager.AddToRoleAsync(demoUser, "User");

            // --- Genres ---
            if (!context.Genres.Any())
            {
                context.Genres.AddRange(
                    new Genre { Name = "Rock" },
                    new Genre { Name = "Jazz" },
                    new Genre { Name = "Pop" },
                    new Genre { Name = "Hip-hop" }
                );
            }

            // --- Track Categories ---
            if (!context.TrackCategories.Any())
            {
                context.TrackCategories.AddRange(
                    new TrackCategory { Name = "Cover" },
                    new TrackCategory { Name = "Instrumental" },
                    new TrackCategory { Name = "Original" }
                );
            }

            // --- First Track ---
            if (!context.MusicTracks.Any())
            {
                context.MusicTracks.Add(new MusicTrack
                {
                    Title = "First Track",
                    Description = "This is a demo track.",
                    FileUrl = "https://example.com/track.mp3",
                    UserId = adminUser.Id,
                    UploadedAt = DateTime.UtcNow
                });
            }

            await context.SaveChangesAsync();
        }

        public static async Task InitializeAsync(IServiceProvider serviceProvider) { }
    }
}
