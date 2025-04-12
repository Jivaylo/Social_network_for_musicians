using System.ComponentModel.DataAnnotations;

namespace SocialNetworkMusician.Models
{
    public class ReportViewModel
    {
        public Guid Id { get; set; }
        public string? ReporterEmail { get; set; }
        public string? ReportedUserEmail { get; set; }
        public string? TrackTitle { get; set; }
        [Required(ErrorMessage = "Please enter a reason for the report.")]
        public string Reason { get; set; } = null!;
        public DateTime ReportedAt { get; set; }
        public string? ReportedUserId { get; set; }
        public Guid? TrackId { get; set; }
    }
}
