using Microsoft.EntityFrameworkCore;
using SocialNetworkForMusician.Data.Entities;
using SocialNetworkForMusician.Data.Models.Entities;

namespace SocialNetworkForMusician.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Album>(entity =>
            {
                entity.HasKey(a => a.AlbumId);
                entity.Property(a => a.Title).IsRequired().HasMaxLength(100);
                entity.Property(a => a.ReleaseDate).IsRequired();
                entity.Property(a => a.CoverImage).HasMaxLength(255);
                entity.HasOne(a => a.Artist)
                      .WithMany(u => u.Albums)
                      .HasForeignKey(a => a.ArtistId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

           
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.CommentId);
                entity.Property(c => c.Content).IsRequired();
                entity.HasOne(c => c.User)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                
            });

           
            modelBuilder.Entity<Follow>(entity =>
            {
                entity.HasKey(f => new { f.FollowerId, f.FollowingId });
                entity.HasOne(f => f.Follower)
                      .WithMany(u => u.Following)
                      .HasForeignKey(f => f.FollowerId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(f => f.Following)
                      .WithMany(u => u.Followers)
                      .HasForeignKey(f => f.FollowingId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

           
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(g => g.GenreId);
                entity.Property(g => g.Name).IsRequired().HasMaxLength(50);
            });

            
            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(l => new { l.UserId, l.TrackId });
                entity.HasOne(l => l.User)
                      .WithMany(u => u.Likes)
                      .HasForeignKey(l => l.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(l => l.Track)
                      .WithMany(t => t.Likes)
                      .HasForeignKey(l => l.TrackId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

           
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.NotificationId);
                entity.Property(n => n.Message).IsRequired().HasMaxLength(500);
                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

          
            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.HasKey(p => p.PlaylistId);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(50);
                entity.HasOne(p => p.User)
                      .WithMany(u => u.Playlists)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        
            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasKey(t => t.TrackId);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(100);
                entity.HasOne(t => t.Album) 
                      .WithMany() 
                      .HasForeignKey(t => t.AlbumId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
