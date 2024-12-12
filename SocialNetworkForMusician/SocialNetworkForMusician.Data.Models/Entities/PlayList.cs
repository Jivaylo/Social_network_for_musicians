using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkForMusician.Data.Models.Entities
{
    public class Playlist
    {
        [Key]
        public int PlaylistId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(255)]
        public string Description { get; set; } = null!;

        [Required]
        public int UserId { get; set; } 

        [Required]
        public DateTime DateCreated { get; set; }


        public User User { get; set; } = null!;
        public ICollection<Track> Tracks { get; set; } = null!;
    }

}
