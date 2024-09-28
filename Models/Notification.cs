namespace PrixFinance3.Models
{
    public partial class Notification
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public virtual User User { get; set; } = null!;
    }
}