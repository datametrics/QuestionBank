using Microsoft.EntityFrameworkCore;
using QuestionBank.Components.Model;

namespace QuestionBank.Components.Services;

public class QuestionBankContext : DbContext
{
    public QuestionBankContext(DbContextOptions<QuestionBankContext> options)
        : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<IHasTimestamps>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync( cancellationToken);
    }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Flag> Flags { get; set; }
    public DbSet<QuestionResultSet> ResultSets { get; set; }
    
    public DbSet<Topic> Topics { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuestionResultSet>()
            .HasMany(qrs => qrs.QuestionResults)
            .WithOne(qr => qr.QuestionResultSet)
            .HasForeignKey(qr => qr.QuestionResultSetId);
        
        modelBuilder.Entity<Question>()
            .Property(q => q.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Question>()
            .Property(q => q.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}

public interface IHasTimestamps
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
