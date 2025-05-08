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
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        { }
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
            {
                context.Database.Migrate();
            }


            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            if (!await roleManager.RoleExistsAsync("Moderator"))
                await roleManager.CreateAsync(new IdentityRole("Moderator"));

            var adminUsers = await userManager.Users
                .Where(u => u.NormalizedEmail == adminEmail.ToUpper())
                .ToListAsync();

            if (adminUsers.Count > 1)
            {
                for (int i = 1; i < adminUsers.Count; i++)
                    await userManager.DeleteAsync(adminUsers[i]);
            }


            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    DisplayName = "Admin"
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating admin user: {error.Description}");
                    }
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }



            var demoUser = await userManager.FindByEmailAsync(demoEmail);
            if (demoUser == null)
            {
                demoUser = new ApplicationUser
                {
                    UserName = "demoUser",
                    Email = demoEmail,
                    DisplayName = "Demo User"
                };
                await userManager.CreateAsync(demoUser, demoPassword);
            }

            if (!await userManager.IsInRoleAsync(demoUser, "User"))
            {
                await userManager.AddToRoleAsync(demoUser, "User");
            }


            if (!context.Genres.Any())
            {
                context.Genres.AddRange(
                    new Genre { Name = "Rock" },
                    new Genre { Name = "Jazz" },
                    new Genre { Name = "Pop" },
                    new Genre { Name = "Hip-hop" }
                );
            }


            if (!context.TrackCategories.Any())
            {
                context.TrackCategories.AddRange(
                    new TrackCategory { Name = "Cover" },
                    new TrackCategory { Name = "Instrumental" },
                    new TrackCategory { Name = "Original" }
                );
            }


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
    }
}