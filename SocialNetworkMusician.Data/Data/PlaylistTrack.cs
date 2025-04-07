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
        public int Id { get; set; }

        [Required]
        public int PlaylistId { get; set; }

        [ForeignKey("PlaylistId")]
        public Playlist Playlist { get; set; } = null!;

        [Required]
        public int TrackId { get; set; }

        [ForeignKey("TrackId")]
        public Track Track { get; set; } = null!;
    }
}
