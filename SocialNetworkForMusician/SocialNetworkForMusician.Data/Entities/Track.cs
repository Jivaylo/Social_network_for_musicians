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
        [MaxLength(150)]
        public string Title { get; set; } = null!;

        public int AlbumId { get; set; }
        public Album Album { get; set; } = null!;

        public int ArtistId { get; set; }
        public User Artist { get; set; } = null!;

        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
