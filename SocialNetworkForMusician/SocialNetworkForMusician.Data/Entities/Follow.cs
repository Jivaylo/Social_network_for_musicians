using SocialNetworkForMusician.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkForMusician.Data.Entities
{
    public class Follow
    {
        [Key]
        public int FollowId { get; set; }

        public int FollowerId { get; set; }
        public User Follower { get; set; } = null!;

        public int FollowingId { get; set; }
        public User Following { get; set; } = null!;
    }
}
