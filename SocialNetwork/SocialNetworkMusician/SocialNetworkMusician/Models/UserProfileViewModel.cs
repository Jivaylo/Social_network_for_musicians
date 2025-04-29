namespace SocialNetworkMusician.Models
{
    public class UserProfileViewModel
    {
        public string Id { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? JoinedDate { get; set; }
        public string? Bio { get; set; }

        public List<TrackViewModel> Tracks { get; set; } = new();

        public int TrackCount => Tracks.Count;

        public List<PlaylistViewModel> Playlists { get; set; } = new();

        public bool IsFollowing { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
    }

}
