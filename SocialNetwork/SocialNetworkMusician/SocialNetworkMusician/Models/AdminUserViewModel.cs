namespace SocialNetworkMusician.Models
{
    public class AdminUserViewModel
    {
        public string Id { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public bool IsBanned { get; set; }
    }
}
