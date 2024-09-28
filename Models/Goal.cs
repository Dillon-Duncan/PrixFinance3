namespace PrixFinance3.Models
{
    public partial class Goal
    {
        public Guid GoalId { get; set; }
        public Guid UserId { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal Progress { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual User User { get; set; } = null!;
    }
}