using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        [Display(Name = "Music File")]
        public IFormFile? MusicFile { get; set; }
        public string? ImageUrl { get; set; } 
        [BindNever]
        public IFormFile? TrackImage { get; set; }

        [Display(Name = "Music File URL")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string? FileUrl { get; set; }

        [Display(Name = "Category")]
        public Guid? CategoryId { get; set; }

        public string? CategoryName { get; set; }
        public int PlayCount { get; set; }

        public DateTime UploadedAt { get; set; }

        public string? UserName { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }

        public bool IsLikedByCurrentUser { get; set; }
        public bool IsDislikedByCurrentUser { get; set; }


        public List<CommentViewModel> Comments { get; set; } = new();
        [BindNever]
        public string? NewComment { get; set; }
    }
}
