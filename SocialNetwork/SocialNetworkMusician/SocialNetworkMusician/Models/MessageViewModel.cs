namespace SocialNetworkMusician.Models
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; } = null!;
        public string RecipientName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
