using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SocialNetworkForMusician.Data.Models.Entities
{
    public class Track
    {
        [Key]
        public int TrackId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        public int ArtistId { get; set; } 

        public int? AlbumId { get; set; } 

        [MaxLength(50)]
        public string Genre { get; set; } = null!;

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        [Required]
        public string FilePath { get; set; } = null!;

       
        public User Artist { get; set; } = null!;
        public Album Album { get; set; } = null!;
        public ICollection<Playlist> Playlists { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = null!;
        public ICollection<Like> Likes { get; set; } = null!;
    }
}
