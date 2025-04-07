namespace SocialNetworkMusician.Models
{
    public class UserProfileViewModel
    {
        public string Id { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? JoinedDate { get; set; }
        public int TrackCount { get; set; }
    }
}
