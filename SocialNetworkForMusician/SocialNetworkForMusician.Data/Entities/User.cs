using SocialNetworkForMusician.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SocialNetworkForMusician.Data.Models.Entities
{

    public class User : IdentityUser
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public ICollection<Album> Albums { get; set; } = new List<Album>();
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Follow> Followers { get; set; } = new List<Follow>();
        public ICollection<Follow> Following { get; set; } = new List<Follow>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
