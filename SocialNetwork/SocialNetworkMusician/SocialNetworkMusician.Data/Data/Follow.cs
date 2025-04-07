using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.Data
{
    public class Follow
    {

        [Required]
        public string FollowerId { get; set; } = null!;

            [ForeignKey("FollowerId")]
            public ApplicationUser Follower { get; set; } = null!;

        [Required]
            public string FollowedId { get; set; } = null!;

        [ForeignKey("FollowedId")]
            public ApplicationUser Followed { get; set; } = null!;

        public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
        }
    }

