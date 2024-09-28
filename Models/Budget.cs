namespace PrixFinance3.Models
{
    public partial class Budget
    {
        public Guid BudgetId { get; set; }
        public Guid UserId { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Period { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public virtual User User { get; set; } = null!;
    }
}