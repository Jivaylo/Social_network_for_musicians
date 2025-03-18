using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkForMusician.Data.Models.Entities
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int TrackId { get; set; }
        public Track Track { get; set; } = null!;
    }
}
