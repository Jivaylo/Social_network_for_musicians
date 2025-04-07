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
    public class Like
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        [JsonIgnore]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public Guid MusicTrackId { get; set; }

        [ForeignKey("MusicTrackId")]
        [JsonIgnore]
        public MusicTrack MusicTrack { get; set; } = null!;

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}
