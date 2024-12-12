using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkForMusician.Data.Models.Entities
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; } = null!;

        [Required]
        public int UserId { get; set; } 

        [Required]
        public int TrackId { get; set; } 

        [Required]
        public DateTime DatePosted { get; set; }

     
        public User User { get; set; } = null!;
        public Track Track { get; set; } = null!;
    }
}
