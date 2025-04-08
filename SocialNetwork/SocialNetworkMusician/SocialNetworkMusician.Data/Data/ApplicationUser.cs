using Microsoft.AspNetCore.Identity;
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
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string? DisplayName { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        public ICollection<MusicTrack> MusicTracks { get; set; } = new List<MusicTrack>();

        [JsonIgnore]
        public ICollection<MusicTrack> Tracks { get; set; } = new List<MusicTrack>();

        [JsonIgnore]
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

        [InverseProperty("Follower")]
        [JsonIgnore]
        public ICollection<Follow> Following { get; set; } = new List<Follow>();

        [InverseProperty("Followed")]
        [JsonIgnore]
        public ICollection<Follow> Followers { get; set; } = new List<Follow>();
    }
}
