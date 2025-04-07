using System.ComponentModel.DataAnnotations;

namespace SocialNetworkMusician.Models
{
    public class TrackViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Track Title")]
        public string Title { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Music File URL")]
        public string FileUrl { get; set; } = null!;

        [Display(Name = "Category")]
        public Guid? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public DateTime UploadedAt { get; set; }

        public string? UserName { get; set; }
    }
}
