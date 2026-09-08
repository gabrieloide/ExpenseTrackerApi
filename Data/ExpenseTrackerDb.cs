using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ExpenseTrackerApi;
using Microsoft.EntityFrameworkCore;

public class ExpenseTrackerDb : IdentityDbContext
{
    public ExpenseTrackerDb(DbContextOptions<ExpenseTrackerDb> options) : base(options) { }
    public DbSet<Expense> Expenses { get; set; }
    public  DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.Property(e => e.Description)
                .IsRequired();
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.HasIndex(e => e.Name)
                .IsUnique();
        });
    }
}