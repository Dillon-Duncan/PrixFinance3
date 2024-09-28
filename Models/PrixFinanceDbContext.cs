using Microsoft.EntityFrameworkCore;

namespace PrixFinance3.Models
{
    public partial class PrixFinanceDbContext : DbContext
    {
        public PrixFinanceDbContext()
        { }

        public PrixFinanceDbContext(DbContextOptions<PrixFinanceDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Budget> Budgets { get; set; }
        public virtual DbSet<Gamification> Gamifications { get; set; }
        public virtual DbSet<Goal> Goals { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<SecuritySetting> SecuritySettings { get; set; }
        public virtual DbSet<SyncData> SyncData { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Budget>(entity =>
            {
                entity.HasKey(e => e.BudgetId).HasName("PK__Budgets__E38E79C4BB1852AA");
                entity.Property(e => e.BudgetId).HasDefaultValueSql("(newid())").HasColumnName("BudgetID");
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.Period).HasMaxLength(20).HasDefaultValue("Monthly");
                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.HasOne(d => d.User).WithMany(p => p.Budgets).HasForeignKey(d => d.UserId).HasConstraintName("FK__Budgets__UserID__693CA210");
            });

            modelBuilder.Entity<Gamification>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Gamifica__1788CCAC2F51A5ED");
                entity.ToTable("Gamification");
                entity.Property(e => e.UserId).ValueGeneratedNever().HasColumnName("UserID");
                entity.HasOne(d => d.User).WithOne(p => p.Gamification).HasForeignKey<Gamification>(d => d.UserId).HasConstraintName("FK__Gamificat__UserI__72C60C4A");
            });

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.HasKey(e => e.GoalId).HasName("PK__Goals__8A4FFF31E2ACFB7E");
                entity.Property(e => e.GoalId).HasDefaultValueSql("(newid())").HasColumnName("GoalID");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.Deadline).HasColumnType("datetime");
                entity.Property(e => e.Progress).HasDefaultValueSql("((0.00))").HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TargetAmount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.HasOne(d => d.User).WithMany(p => p.Goals).HasForeignKey(d => d.UserId).HasConstraintName("FK__Goals__UserID__6EF57B66");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E3229C4D940");
                entity.Property(e => e.NotificationId).HasDefaultValueSql("(newid())").HasColumnName("NotificationID");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.Message).HasMaxLength(512);
                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.HasOne(d => d.User).WithMany(p => p.Notifications).HasForeignKey(d => d.UserId).HasConstraintName("FK__Notificat__UserI__787EE5A0");
            });

            modelBuilder.Entity<SecuritySetting>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Security__1788CCAC0460D12B");
                entity.Property(e => e.UserId).ValueGeneratedNever().HasColumnName("UserID");
                entity.Property(e => e.EncryptionKey).HasMaxLength(512);
                entity.Property(e => e.LastUpdated).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.HasOne(d => d.User).WithOne(p => p.SecuritySetting).HasForeignKey<SecuritySetting>(d => d.UserId).HasConstraintName("FK__SecurityS__UserI__00200768");
            });

            modelBuilder.Entity<SyncData>(entity =>
            {
                entity.HasKey(e => e.SyncId).HasName("PK__SyncData__7E50DEA6A9D0D99D");
                entity.Property(e => e.SyncId).HasDefaultValueSql("(newid())").HasColumnName("SyncID");
                entity.Property(e => e.LastSyncDate).HasColumnType("datetime");
                entity.Property(e => e.SyncStatus).HasMaxLength(50);
                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.HasOne(d => d.User).WithMany(p => p.SyncData).HasForeignKey(d => d.UserId).HasConstraintName("FK__SyncData__UserID__7C4F7684");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B16BEDA6F");
                entity.Property(e => e.TransactionId).HasDefaultValueSql("(newid())").HasColumnName("TransactionID");
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e => e.Date).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(512);
                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.HasOne(d => d.User).WithMany(p => p.Transactions).HasForeignKey(d => d.UserId).HasConstraintName("FK__Transacti__UserI__6383C8BA");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACB9F16EC9");
                entity.Property(e => e.UserId).HasDefaultValueSql("(newid())").HasColumnName("UserID");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.PasswordHash).HasMaxLength(512);
                entity.Property(e => e.PreferredLanguage).HasMaxLength(10).HasDefaultValue("English");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}