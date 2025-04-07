using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.Data
{
    public class TrackCategory
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public ICollection<MusicTrack> Tracks { get; set; } = new List<MusicTrack>();
    }
}
