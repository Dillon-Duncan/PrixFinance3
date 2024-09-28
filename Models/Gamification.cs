namespace PrixFinance3.Models
{
    public partial class Gamification
    {
        public Guid UserId { get; set; }
        public int Points { get; set; }
        public string? Achievements { get; set; }
        public virtual User User { get; set; } = null!;
    }
}