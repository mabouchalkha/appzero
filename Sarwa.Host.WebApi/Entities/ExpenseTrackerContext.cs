using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sarwa.Host.WebApi.Entities
{
    public class ExpenseTrackerContext : DbContext
    {
        public ExpenseTrackerContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseGroup> ExpenseGroups { get; set; }
        public DbSet<ExpenseGroupStatus> ExpenseGroupStatusses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount);

            modelBuilder.Entity<ExpenseGroup>()
                .HasMany(e => e.Expenses)
                .WithOne(e => e.ExpenseGroup)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExpenseGroupStatus>()
                .HasMany(e => e.ExpenseGroups)
                .WithOne(e => e.ExpenseGroupStatus)
                .HasForeignKey(e => e.ExpenseGroupStatusId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
