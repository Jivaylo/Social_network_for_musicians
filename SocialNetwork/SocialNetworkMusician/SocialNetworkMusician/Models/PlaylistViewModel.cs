using System.ComponentModel.DataAnnotations;

namespace SocialNetworkMusician.Models
{
    public class PlaylistViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        public List<TrackViewModel> Tracks { get; set; } = new();
    }
}
