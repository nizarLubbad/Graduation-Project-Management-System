using Microsoft.EntityFrameworkCore;

namespace GPMS_MALAK.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<KanbanTask> Tasks { get; set; }
        public DbSet<StudentTask> StudentTasks { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .Property(s => s.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .Property(s => s.Password)
                .HasColumnType("varchar(100)");

            modelBuilder.Entity<StudentTask>()
                .HasKey(st => new { st.StudentId, st.TaskId });

            modelBuilder.Entity<StudentTask>()
                .HasOne(st => st.Student)
                .WithMany(s => s.StudentTasks)
                .HasForeignKey(st => st.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentTask>()
                .HasOne(st => st.Task)
                .WithMany(t => t.StudentTasks)
                .HasForeignKey(st => st.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Link>()
                .HasOne(l => l.KanbanTask)
                .WithMany(t => t.Links)
                .HasForeignKey(l => l.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.KanbanTask)
                .WithMany(t => t.Feedbacks)
                .HasForeignKey(f => f.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
