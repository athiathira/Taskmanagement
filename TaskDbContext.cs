using Microsoft.EntityFrameworkCore;
using Task = TaskManagement.Models.Task; // Alias to avoid ambiguity

namespace TaskManagement.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>(eb =>
            {
                eb.HasKey(t => t.Id);
                eb.Property(t => t.Title).IsRequired().HasMaxLength(200);
                eb.Property(t => t.Description).HasMaxLength(1000);
                eb.Property(t => t.IsCompleted);
                eb.Property(t => t.Priority);
            });
        }
    }
}

