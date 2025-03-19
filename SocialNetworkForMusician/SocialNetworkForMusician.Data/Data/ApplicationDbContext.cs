using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetworkForMusician.Data.Entities;
using SocialNetworkForMusician.Data.Models.Entities;

namespace SocialNetworkForMusician.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Artist>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<Artist>(a => a.UserId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Song)
                .WithMany(s => s.Ratings)
                .HasForeignKey(r => r.SongId);

            modelBuilder.Entity<Leaderboard>()
                .HasOne(l => l.Song)
                .WithMany()
                .HasForeignKey(l => l.SongId);

            modelBuilder.Entity<Genre>()
                .HasMany(g => g.Songs)
                .WithOne(s => s.Genre)
                .HasForeignKey(s => s.GenreId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
