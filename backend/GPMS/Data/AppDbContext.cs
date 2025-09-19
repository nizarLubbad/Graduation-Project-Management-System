using GPMS.Models;
using GPMS.Services;
using Microsoft.EntityFrameworkCore;
namespace GPMS.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
       

        public DbSet<Student> Students { get; set; }
        public DbSet<KanbanTask> Tasks { get; set; }
        public DbSet<StudentTask> StudentTasks { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<Reply> Replys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary keys
            modelBuilder.Entity<KanbanTask>().HasKey(t => t.Id);
            modelBuilder.Entity<Team>().HasKey(t => t.TeamId);
            modelBuilder.Entity<Student>().HasKey(s => s.StudentId);
            modelBuilder.Entity<Supervisor>().HasKey(s => s.SupervisorId);
            //make project id auto increment and unique
            modelBuilder.Entity<Project>()
                .HasIndex(p => p.Id)
                .IsUnique();

            // 

            //
            modelBuilder.Entity<Project>()
                .HasKey(p => p.ProjectTitle);

            modelBuilder.Entity<Project>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // make TeamName unique
            modelBuilder.Entity<Team>()
                .HasIndex(t => t.TeamName)
                .IsUnique();

            // 1. Team -> KanbanTasks (One-to-Many)
            modelBuilder.Entity<Team>()
                .HasMany(t => t.KanbanTasks)
                .WithOne(t => t.Team)
                .HasForeignKey(t => t.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2. Team -> Students (One-to-Many) - العلاقة الجديدة
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Students)
                .WithOne(s => s.Team)
                .HasForeignKey(s => s.TeamId)
                .OnDelete(DeleteBehavior.SetNull); //

            // 3. Team -> Project (One-to-One)
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Project)
                .WithOne(p => p.Team)
                .HasForeignKey<Project>(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade); // 

            // 4. Supervisor -> Teams (One-to-Many)
            modelBuilder.Entity<Supervisor>()
                .HasMany(s => s.Teams)
                .WithOne(t => t.Supervisor)
                .HasForeignKey(t => t.SupervisorId)
                .OnDelete(DeleteBehavior.SetNull);
            // 5. Supervisor -> Feedback (One-to-Many)
            modelBuilder.Entity<Supervisor>()
                .HasMany(s => s.Feedbacks)
                .WithOne(t => t.Supervisor)
                .HasForeignKey(t => t.SupervisorId)
                .OnDelete(DeleteBehavior.SetNull);



            // Student configuration
            modelBuilder.Entity<Student>()
                .Property(s => s.StudentId)
                .ValueGeneratedNever();
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique();

            // Supervisor configuration
            modelBuilder.Entity<Supervisor>()
                .HasIndex(s => s.Email)
                .IsUnique();

            // StudentTask many-to-many relationship
            modelBuilder.Entity<StudentTask>()
                .HasKey(st => new { st.StudentId, st.TaskId });

            modelBuilder.Entity<StudentTask>()
                .HasOne(st => st.Student)
                .WithMany(s => s.StudentTask)
                .HasForeignKey(st => st.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentTask>()
                .HasOne(st => st.Task)
                .WithMany(t => t.StudentTasks)
                .HasForeignKey(st => st.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            Cascade delete configurations
            modelBuilder.Entity<KanbanTask>()
                .HasMany(t => t.Links)
                .WithOne(l => l.KanbanTask)
                .HasForeignKey(l => l.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KanbanTask>()
                .HasMany(t => t.Feedbacks)
                .WithOne(f => f.KanbanTask)
                .HasForeignKey(f => f.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            // one - many between Reply and Supervisor
            modelBuilder.Entity<Supervisor>()
                .HasMany(s => s.Replys)
                .WithOne(r => r.Supervisor)
                .HasForeignKey(r => r.SupervisorId)
                .OnDelete(DeleteBehavior.Cascade);
            // one - many between Reply and Feedback
            modelBuilder.Entity<Feedback>()
                .HasMany(f => f.Replys)
                .WithOne(r => r.Feedback)
                .HasForeignKey(r => r.FeedbackId)
                .OnDelete(DeleteBehavior.Cascade);
            //one - many between Reply and
            modelBuilder.Entity<Student>()
             .HasMany(f => f.Replys)
             .WithOne(r => r.Student)
             .HasForeignKey(r => r.StudentId)
             .OnDelete(DeleteBehavior.Cascade);

        }




    }
}