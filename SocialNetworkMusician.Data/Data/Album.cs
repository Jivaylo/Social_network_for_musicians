using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.Data
{
    public class Album
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string CoverImageUrl { get; set; } = null!;

        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public ICollection<Track> Tracks { get; set; } = null!;
    }
}
