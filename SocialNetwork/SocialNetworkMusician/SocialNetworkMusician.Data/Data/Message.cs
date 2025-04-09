using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.Data
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string SenderId { get; set; } = null!;
        [ForeignKey("SenderId")]
        public ApplicationUser Sender { get; set; } = null!;

        [Required]
        public string RecipientId { get; set; } = null!;
        [ForeignKey("RecipientId")]
        public ApplicationUser Recipient { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = null!;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
