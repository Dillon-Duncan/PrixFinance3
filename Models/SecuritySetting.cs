namespace PrixFinance3.Models
{
    public partial class SecuritySetting
    {
        public Guid UserId { get; set; }
        public string EncryptionKey { get; set; } = string.Empty;
        public DateTime? LastUpdated { get; set; }
        public virtual User User { get; set; } = null!;
    }
}