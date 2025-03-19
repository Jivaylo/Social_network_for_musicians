using SocialNetworkForMusician.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkForMusician.Data.Entities
{
    public class Artist
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string StageName { get; set; }
        [Required]
        public string Bio { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}
