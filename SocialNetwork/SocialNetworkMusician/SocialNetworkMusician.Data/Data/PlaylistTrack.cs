using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.Data
{
    public class PlaylistTrack
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PlaylistId { get; set; }
        [ForeignKey("PlaylistId")]
        public Playlist Playlist { get; set; } = null!;

        public Guid MusicTrackId { get; set; }
        [ForeignKey("MusicTrackId")]
        public MusicTrack MusicTrack { get; set; } = null!;
    }
}
