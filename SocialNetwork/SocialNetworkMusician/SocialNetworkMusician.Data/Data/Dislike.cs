using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.Data
{
    public class Dislike
    {
        [Key]
        public Guid Id { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public Guid MusicTrackId { get; set; }
        public MusicTrack MusicTrack { get; set; } = null!;
    }
}
