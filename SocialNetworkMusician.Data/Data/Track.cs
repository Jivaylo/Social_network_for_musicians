using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.Data
{
    public class Track
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string FilePath { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
        public int? AlbumId { get; set; }
        public Album? Album { get; set; }


        public ICollection<Comment> Comments { get; set; } = null!;
        public ICollection<Like> Likes { get; set; } = null!;
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = null!;
        public DateTime UploadedAt { get; set; }
        public object Category { get; set; }
        public object CategoryId { get; set; }
    }
}
