




//////    }
//////}
////using GPMS.Models;
////using Microsoft.EntityFrameworkCore;

////namespace GPMS.Models
////{
////    public class AppDbContext : DbContext
////    {
////        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

////        public DbSet<Student> Students { get; set; }
////        public DbSet<KanbanTask> Tasks { get; set; }
////        public DbSet<StudentTask> StudentTasks { get; set; }
////        public DbSet<Link> Links { get; set; }
////        public DbSet<Feedback> Feedbacks { get; set; }
////        public DbSet<Team> Teams { get; set; }
////        public DbSet<Project> Projects { get; set; }
////        public DbSet<Supervisor> Supervisors { get; set; }
////        public DbSet<Reply> Replys { get; set; }
////        public DbSet<User> Users { get; set; } = null!;

////        protected override void OnModelCreating(ModelBuilder modelBuilder)
////        {
////            // ---------------- Primary Keys ----------------
////            modelBuilder.Entity<KanbanTask>().HasKey(t => t.Id);
////            modelBuilder.Entity<Team>().HasKey(t => t.TeamId);
////            modelBuilder.Entity<Student>().HasKey(s => s.StudentId);
////            modelBuilder.Entity<Supervisor>().HasKey(s => s.SupervisorId);

////            // ---------------- Project ----------------
////            modelBuilder.Entity<Project>()
////                .HasKey(p => p.ProjectTitle); // Primary Key هو ProjectTitle

////            modelBuilder.Entity<Project>()
////                .Property(p => p.Id)
////                .ValueGeneratedOnAdd(); // Id auto increment

////            modelBuilder.Entity<Project>()
////                .HasIndex(p => p.Id)
////                .IsUnique(); // Id فريد

////            // ---------------- Team ----------------
////            modelBuilder.Entity<Team>()
////                .HasIndex(t => t.TeamName)
////                .IsUnique();

////            modelBuilder.Entity<Team>()
////                .HasMany(t => t.KanbanTasks)
////                .WithOne(t => t.Team)
////                .HasForeignKey(t => t.TeamId)
////                .OnDelete(DeleteBehavior.Cascade);

////            modelBuilder.Entity<Team>()
////                .HasMany(t => t.Students)
////                .WithOne(s => s.Team)
////                .HasForeignKey(s => s.TeamId)
////                .OnDelete(DeleteBehavior.SetNull);

////            modelBuilder.Entity<Team>()
////                .HasOne(t => t.Project)
////                .WithOne(p => p.Team)
////                .HasForeignKey<Project>(p => p.TeamId)
////                .OnDelete(DeleteBehavior.Cascade);

////            // ---------------- Supervisor ----------------
////            modelBuilder.Entity<Supervisor>()
////                .HasMany(s => s.Teams)
////                .WithOne(t => t.Supervisor)
////                .HasForeignKey(t => t.SupervisorId)
////                .OnDelete(DeleteBehavior.SetNull);

////            modelBuilder.Entity<Supervisor>()
////                .HasMany(s => s.Feedbacks)
////                .WithOne(f => f.Supervisor)
////                .HasForeignKey(f => f.SupervisorId)
////                .OnDelete(DeleteBehavior.SetNull);

////            // ---------------- Student & Supervisor configuration ----------------
////            modelBuilder.Entity<Student>()
////                .Property(s => s.StudentId)
////                .ValueGeneratedNever(); // <-- ادخاله يدوي

////            modelBuilder.Entity<Student>()
////                .HasIndex(s => s.Email)
////                .IsUnique();

////            modelBuilder.Entity<Supervisor>()
////                .HasIndex(s => s.Email)
////                .IsUnique();

////            // ---------------- StudentTask many-to-many ----------------
////            modelBuilder.Entity<StudentTask>()
////                .HasKey(st => new { st.StudentId, st.TaskId });

////            modelBuilder.Entity<StudentTask>()
////                .HasOne(st => st.Student)
////                .WithMany(s => s.StudentTask)
////                .HasForeignKey(st => st.StudentId)
////                .OnDelete(DeleteBehavior.Restrict);

////            modelBuilder.Entity<StudentTask>()
////                .HasOne(st => st.Task)
////                .WithMany(t => t.StudentTasks)
////                .HasForeignKey(st => st.TaskId)
////                .OnDelete(DeleteBehavior.Restrict);

////            // ---------------- KanbanTask relationships ----------------
////            modelBuilder.Entity<KanbanTask>()
////                .HasMany(t => t.Links)
////                .WithOne(l => l.KanbanTask)
////                .HasForeignKey(l => l.TaskId)
////                .OnDelete(DeleteBehavior.Cascade);

////            modelBuilder.Entity<KanbanTask>()
////                .HasMany(t => t.Feedbacks)
////                .WithOne(f => f.KanbanTask)
////                .HasForeignKey(f => f.TaskId)
////                .OnDelete(DeleteBehavior.Restrict);

////            // ---------------- Reply relationships ----------------
////            modelBuilder.Entity<Reply>()
////                .HasOne(r => r.Supervisor)
////                .WithMany(s => s.Replys)
////                .HasForeignKey(r => r.SupervisorId)
////                .OnDelete(DeleteBehavior.Cascade);

////            modelBuilder.Entity<Reply>()
////                .HasOne(r => r.Feedback)
////                .WithMany(f => f.Replys)
////                .HasForeignKey(r => r.FeedbackId)
////                .OnDelete(DeleteBehavior.Cascade);

////            modelBuilder.Entity<Reply>()
////                .HasOne(r => r.Student)
////                .WithMany(s => s.Replys)
////                .HasForeignKey(r => r.StudentId)
////                .OnDelete(DeleteBehavior.Cascade);

////            // ---------------- User relationships ----------------
////            modelBuilder.Entity<User>()
////                .HasOne(u => u.Student)
////                .WithOne(s => s.User)
////                .HasForeignKey<Student>(s => s.UserId)
////                .OnDelete(DeleteBehavior.Cascade);

////            modelBuilder.Entity<User>()
////                .HasOne(u => u.Supervisor)
////                .WithOne(sp => sp.User)
////                .HasForeignKey<Supervisor>(sp => sp.UserId)
////                .OnDelete(DeleteBehavior.Cascade);
////        }
////    }
////}
//using GPMS.Models;
//using Microsoft.EntityFrameworkCore;

using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<KanbanTask> Tasks { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<Reply> Replys { get; set; }
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---------------- Primary Keys ----------------
            modelBuilder.Entity<KanbanTask>().HasKey(t => t.Id);
            modelBuilder.Entity<Team>().HasKey(t => t.TeamId);
            modelBuilder.Entity<Student>().HasKey(s => s.StudentId);
            modelBuilder.Entity<Supervisor>().HasKey(s => s.SupervisorId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);

            // IDs manually assigned (not auto-increment)
            modelBuilder.Entity<Student>().Property(s => s.StudentId).ValueGeneratedNever();
            modelBuilder.Entity<Team>().Property(t => t.TeamId).ValueGeneratedNever();
            modelBuilder.Entity<Supervisor>().Property(s => s.SupervisorId).ValueGeneratedNever();
            modelBuilder.Entity<User>().Property(u => u.UserId).ValueGeneratedNever();

            // ---------------- Project ----------------
            modelBuilder.Entity<Project>()
                .HasKey(p => p.ProjectTitle);

            modelBuilder.Entity<Project>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd(); // Project Id auto increment

            modelBuilder.Entity<Project>()
                .HasIndex(p => p.Id)
                .IsUnique();

            // ---------------- Team ----------------
            modelBuilder.Entity<Team>()
                .HasIndex(t => t.TeamName)
                .IsUnique();

            modelBuilder.Entity<Team>()
                .HasMany(t => t.KanbanTasks)
                .WithOne(t => t.Team)
                .HasForeignKey(t => t.TeamId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Team>()
                .HasMany(t => t.Students)
                .WithOne(s => s.Team)
                .HasForeignKey(s => s.TeamId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Project)
                .WithOne(p => p.Team)
                .HasForeignKey<Project>(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade); 

            // ---------------- Supervisor ----------------
            modelBuilder.Entity<Supervisor>()
                .HasMany(s => s.Teams)
                .WithOne(t => t.Supervisor)
                .HasForeignKey(t => t.SupervisorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Supervisor>()
                .HasMany(s => s.Feedbacks)
                .WithOne(f => f.Supervisor)
                .HasForeignKey(f => f.SupervisorId)
                .OnDelete(DeleteBehavior.SetNull);
            // ---------------- Links relationships ----------------
            modelBuilder.Entity<Link>()
                .HasOne(l => l.Student)
                .WithMany(s => s.Links)
                .HasForeignKey(l => l.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Link>()
                .HasOne(l => l.Team)
                .WithMany(t => t.Links)
                .HasForeignKey(l => l.TeamId)
                .OnDelete(DeleteBehavior.Restrict); 

            // ---------------- Student & Supervisor indexes ----------------
            modelBuilder.Entity<Student>().HasIndex(s => s.Email).IsUnique();
            modelBuilder.Entity<Supervisor>().HasIndex(s => s.Email).IsUnique();

            // ---------------- Reply ----------------
            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Replies)
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Supervisor)
                .WithMany(s => s.Replys)
                .HasForeignKey(r => r.SupervisorId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Feedback)
                .WithMany(f => f.Replies)
                .HasForeignKey(r => r.FeedbackId)
                .OnDelete(DeleteBehavior.Cascade); 

            // ---------------- User relationships ----------------
            modelBuilder.Entity<User>()
                .HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Supervisor)
                .WithOne(sp => sp.User)
                .HasForeignKey<Supervisor>(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}




