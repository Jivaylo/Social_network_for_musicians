using System.ComponentModel.DataAnnotations;

namespace SocialNetworkMusician.Models
{
    public class ComposeMessageViewModel
    {
        public string RecipientId { get; set; } = null!;
        public string RecipientName { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = null!;
    }
}
