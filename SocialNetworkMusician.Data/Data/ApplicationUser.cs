using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string DisplayName { get; set; } = null!;

        [MaxLength(500)]
        public string Bio { get; set; } = null!;

        public string ProfilePictureUrl { get; set; } = null!;


        public ICollection<Track> Tracks { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = null!;
        public ICollection<Like> Likes { get; set; } = null!;
        public ICollection<Message> SentMessages { get; set; } = null!;
        public ICollection<Message> ReceivedMessages { get; set; } = null!;
        public ICollection<Follow> Followers { get; set; } = null!;
        public ICollection<Follow> Following { get; set; } = null!;
        public ICollection<Playlist> Playlists { get; set; } = null!;
        public ICollection<Album> Albums { get; set; } = null!;
    }
}
