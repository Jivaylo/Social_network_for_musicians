using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkForMusician.Data.Entities
{
    public class Leaderboard
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SongId { get; set; }
        [Required]
        public double AverageRating { get; set; }
        public virtual Song Song { get; set; }
    }
}
