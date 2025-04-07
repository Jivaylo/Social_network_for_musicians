using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.Seed
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            context.Database.Migrate();

            // Seed User
            if (!userManager.Users.Any())
            {
                var demoUser = new ApplicationUser
                {
                    UserName = "demo@music.com",
                    Email = "demo@music.com",
                    EmailConfirmed = true
                };

                userManager.CreateAsync(demoUser, "Demo123!").Wait();
            }

           
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Rock" },
                    new Category { Name = "Jazz" },
                    new Category { Name = "Hip-Hop" }
                );
                context.SaveChanges();
            }

           
            if (!context.Albums.Any())
            {
                var album = new Album
                {
                    Title = "Demo Album",
                    ReleaseDate = DateTime.UtcNow,
                    Description = "This is a demo album."
                };
                context.Albums.Add(album);
                context.SaveChanges();
            }
        }
    }
}
