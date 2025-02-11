using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkForMusician.Data.Models.Entities
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        public int ArtistId { get; set; } 

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Url]
        public string CoverImage { get; set; } = null!;

  
        public User Artist { get; set; } = null!;
        public ICollection<Track> Tracks { get; set; } = null!;
    }
}
