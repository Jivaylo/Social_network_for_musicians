using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialNetworkMusician.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.SeedData
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();

            // Seed Roles
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            // Seed Users
            var adminUser = await userManager.FindByEmailAsync("admin@music.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@music.com",
                    DisplayName = "Admin"
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var demoUser = await userManager.FindByEmailAsync("user@music.com");
            if (demoUser == null)
            {
                demoUser = new ApplicationUser
                {
                    UserName = "demoUser",
                    Email = "user@music.com",
                    DisplayName = "Demo User"
                };
                await userManager.CreateAsync(demoUser, "User123!");
                await userManager.AddToRoleAsync(demoUser, "User");
            }

            // Seed Genres
            if (!context.Genres.Any())
            {
                context.Genres.AddRange(
                    new Genre { Name = "Rock" },
                    new Genre { Name = "Jazz" },
                    new Genre { Name = "Pop" },
                    new Genre { Name = "Hip-hop" }
                );
            }

            // Seed Categories
            if (!context.TrackCategories.Any())
            {
                context.TrackCategories.AddRange(
                    new TrackCategory { Name = "Cover" },
                    new TrackCategory { Name = "Instrumental" },
                    new TrackCategory { Name = "Original" }
                );
            }

            // Seed Music Tracks
            if (!context.MusicTracks.Any())
            {
                context.MusicTracks.Add(new MusicTrack
                {
                    Title = "First Track",
                    Description = "This is a demo track.",
                    FileUrl = "https://example.com/track.mp3",
                    UserId = demoUser.Id
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
