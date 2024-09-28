namespace PrixFinance3.Models
{
    public partial class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public byte[]? BiometricData { get; set; }
        public string? PreferredLanguage { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public virtual Gamification? Gamification { get; set; }
        public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual SecuritySetting? SecuritySetting { get; set; }
        public virtual ICollection<SyncData> SyncData { get; set; } = new List<SyncData>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}