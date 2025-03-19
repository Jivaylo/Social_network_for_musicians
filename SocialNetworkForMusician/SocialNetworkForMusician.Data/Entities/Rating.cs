using SocialNetworkForMusician.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkForMusician.Data.Entities
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int SongId { get; set; }
        [Required]
        [Range(1, 5)]
        public int Score { get; set; }
        public virtual User User { get; set; }
        public virtual Song Song { get; set; }
    }
}
