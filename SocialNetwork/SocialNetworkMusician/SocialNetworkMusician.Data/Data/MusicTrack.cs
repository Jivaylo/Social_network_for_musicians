using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.Data
{
    public class MusicTrack
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }


        [Required, Url]
        public string FileUrl { get; set; } = null!;

        [DataType(DataType.DateTime)]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        [JsonIgnore]
        public ApplicationUser User { get; set; } = null!;

        public Guid? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public TrackCategory? Category { get; set; }

        [JsonIgnore]
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Dislike> Dislikes { get; set; } = new List<Dislike>();

        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        [JsonIgnore]
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();

        [JsonIgnore]
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();
    }
}
