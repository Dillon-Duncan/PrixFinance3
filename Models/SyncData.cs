namespace PrixFinance3.Models
{
    public partial class SyncData
    {
        public Guid SyncId { get; set; }
        public Guid UserId { get; set; }
        public string SyncStatus { get; set; } = string.Empty;
        public DateTime? LastSyncDate { get; set; }
        public string? DataChanges { get; set; }
        public virtual User User { get; set; } = null!;
    }
}