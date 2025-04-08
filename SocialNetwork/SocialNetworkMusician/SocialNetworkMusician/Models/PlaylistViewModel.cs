namespace SocialNetworkMusician.Models
{
    public class PlaylistViewModel
    {   
            public Guid Id { get; set; }
            public string Name { get; set; } = null!;
            public List<TrackViewModel> Tracks { get; set; } = new();
    }
}
