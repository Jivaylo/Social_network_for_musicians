using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.Data
{
    public class Report
    {
        [Key]
        public Guid Id { get; set; }

        public string ReporterId { get; set; } = null!;
        [ForeignKey("ReporterId")]
        public ApplicationUser Reporter { get; set; } = null!;

        public string? ReportedUserId { get; set; }
        [ForeignKey("ReportedUserId")]
        public ApplicationUser? ReportedUser { get; set; }

        public Guid? TrackId { get; set; }
        [ForeignKey("TrackId")]
        public MusicTrack? Track { get; set; }

        [Required]
        public string Reason { get; set; } = null!;

        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        public bool Resolved { get; set; } = false;
    }
}
