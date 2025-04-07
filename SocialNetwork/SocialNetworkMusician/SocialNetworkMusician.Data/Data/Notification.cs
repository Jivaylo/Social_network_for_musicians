using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SocialNetworkMusician.Data.Data
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(300)]
        public string Message { get; set; } = null!;

        public bool IsRead { get; set; } = false;

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        [JsonIgnore]
        public ApplicationUser User { get; set; } = null!;
    }
}
