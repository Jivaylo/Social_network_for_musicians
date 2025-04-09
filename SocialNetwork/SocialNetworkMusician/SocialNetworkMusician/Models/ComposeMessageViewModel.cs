using System.ComponentModel.DataAnnotations;

namespace SocialNetworkMusician.Models
{
    public class ComposeMessageViewModel
    {
        [Required(ErrorMessage = "Recipient is required.")]
        public string RecipientId { get; set; } = null!;

        [Display(Name = "Recipient")]
        public string? RecipientName { get; set; }

        [Required(ErrorMessage = "Message content is required.")]
        [StringLength(1000)]
        public string Content { get; set; } = null!;
    }

}
