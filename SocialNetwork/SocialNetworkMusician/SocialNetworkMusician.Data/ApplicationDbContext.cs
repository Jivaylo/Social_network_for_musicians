﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Data.Data;

namespace SocialNetworkMusician.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public DbSet<MusicTrack> MusicTracks { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<TrackCategory> TrackCategories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Dislike> Dislikes { get; set; }
        public DbSet<Message> Messages { get; set; } 
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Follow>()
                .HasKey(f => new { f.FollowerId, f.FollowedId });

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Followed)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany()
                .HasForeignKey(m => m.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<PlaylistTrack>()
                 .HasKey(pt => pt.Id);

            modelBuilder.Entity<PlaylistTrack>()
                .HasOne(pt => pt.Playlist)
                .WithMany(p => p.PlaylistTracks)
                .HasForeignKey(pt => pt.PlaylistId)
                .OnDelete(DeleteBehavior.Restrict); ;

            modelBuilder.Entity<PlaylistTrack>()
                .HasOne(pt => pt.MusicTrack)
                .WithMany()
                .HasForeignKey(pt => pt.MusicTrackId)
                .OnDelete(DeleteBehavior.Restrict); ;


            modelBuilder.Entity<MusicTrack>()
                .HasMany(m => m.Genres)
                .WithMany(g => g.Tracks)
                .UsingEntity(j => j.ToTable("TrackGenres"));

           
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.MusicTrack)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.MusicTrackId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Like>()
                .HasOne(l => l.MusicTrack)
                .WithMany(t => t.Likes)
                .HasForeignKey(l => l.MusicTrackId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Playlist>()
                .HasOne(p => p.User)
                .WithMany(u => u.Playlists)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<MusicTrack>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tracks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<MusicTrack>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tracks)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dislike>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dislike>()
                .HasOne(d => d.MusicTrack)
                .WithMany(t => t.Dislikes)
                .HasForeignKey(d => d.MusicTrackId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Reporter)
                .WithMany()
                .HasForeignKey(r => r.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.ReportedUser)
                .WithMany()
                .HasForeignKey(r => r.ReportedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Track)
                .WithMany()
                .HasForeignKey(r => r.TrackId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
