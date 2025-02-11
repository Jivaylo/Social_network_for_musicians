using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SocialNetworkForMusician.Data.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Url]
        public string ProfilePicture { get; set; } = null!;

        [Required]
        public DateTime DateJoined { get; set; }

      
        public ICollection<Playlist> Playlists { get; set; } = null!;
        public ICollection<Track> Tracks { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = null!;
        public ICollection<Like> Likes { get; set; } = null!;
        public ICollection<Album> Albums { get; set; } = null!;
    }
}
