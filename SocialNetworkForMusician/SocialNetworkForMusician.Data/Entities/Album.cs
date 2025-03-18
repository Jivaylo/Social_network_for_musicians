using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkForMusician.Data.Models.Entities
{
    public class Album
    {

        [Key]
        public int AlbumId { get; set; }


        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; } = null!;

     
        public string? CoverImage { get; set; }

      
        [Required]
        public DateTime ReleaseDate { get; set; }


        [Required]
        [ForeignKey("Artist")]
        public string ArtistId { get; set; } = null!;

        public virtual User Artist { get; set; } = null!;
    }

}
