namespace PrixFinance3.Models
{
    public partial class Transaction
    {
        public Guid TransactionId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public virtual User User { get; set; } = null!;
    }
}