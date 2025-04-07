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
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(300)]
        public string Content { get; set; } = null!;

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

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
    }
}
