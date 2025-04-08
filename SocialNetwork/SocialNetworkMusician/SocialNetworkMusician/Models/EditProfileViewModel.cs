namespace SocialNetworkMusician.Models
{
    public class EditProfileViewModel
    {
        public string DisplayName { get; set; } = null!;
        public string? Bio { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string? ExistingImageUrl { get; set; }
    }
}
