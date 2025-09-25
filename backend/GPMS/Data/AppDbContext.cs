using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<KanbanTask> Tasks { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Reply> Replys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------- Primary Keys ----------------
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Student>().HasKey(s => s.UserId);
            modelBuilder.Entity<Supervisor>().HasKey(s => s.UserId);
            modelBuilder.Entity<Team>().HasKey(t => t.TeamId);
            modelBuilder.Entity<KanbanTask>().HasKey(t => t.Id);
            modelBuilder.Entity<Project>().HasKey(p => p.Id); 
            modelBuilder.Entity<Feedback>().HasKey(f => f.Id);
            modelBuilder.Entity<Link>().HasKey(l => l.Id);
            modelBuilder.Entity<Reply>().HasKey(r => r.Id);

            // ---------------- Identity settings ----------------
            modelBuilder.Entity<User>()
                 .Property(u => u.UserId)
                 .ValueGeneratedNever();

            //modelBuilder.Entity<Student>()
            //    .Property(s => s.UserId)
            //    .ValueGeneratedNever();

            //modelBuilder.Entity<Supervisor>()
            //    .Property(s => s.UserId)
            //    .ValueGeneratedNever();


            modelBuilder.Entity<Student>()
                     .Property(s => s.UserId)
                     .ValueGeneratedNever();
            modelBuilder.Entity<Feedback>()
                  .Property(f => f.Id)
                  .ValueGeneratedOnAdd();


            modelBuilder.Entity<Supervisor>()
                .Property(s => s.UserId)
                .ValueGeneratedNever();


            modelBuilder.Entity<Team>()
                .Property(t => t.TeamId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Project>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd(); 

            // ---------------- User ↔ Student/Supervisor ----------------
            modelBuilder.Entity<User>()
                     .HasOne(u => u.Student)
                     .WithOne(s => s.User)
                     .HasForeignKey<Student>(s => s.UserId)
                     .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Supervisor)
                .WithOne(s => s.User)
                .HasForeignKey<Supervisor>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------- Team ↔ Students (One-to-Many) ----------------
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Students)
                .WithOne(s => s.Team)
                .HasForeignKey(s => s.TeamId)
                .OnDelete(DeleteBehavior.SetNull); 

            // ---------------- Team ↔ Supervisor (Many-to-One) ----------------
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Supervisor)
                .WithMany(s => s.Teams)
                .HasForeignKey(t => t.SupervisorId)
                .OnDelete(DeleteBehavior.SetNull);

            // ---------------- Feedback ↔ Supervisor & Team ----------------
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Supervisor)
                .WithMany(s => s.Feedbacks)
                .HasForeignKey(f => f.SupervisorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Team)
                .WithMany(t => t.Feedbacks)
                .HasForeignKey(f => f.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------- Student ↔ Links / Replies ----------------
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Links)
                .WithOne(l => l.Student)
                .HasForeignKey(l => l.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                 .HasMany(s => s.Replies)
                 .WithOne(r => r.Student)
                 .HasForeignKey(r => r.StudentId)
                 .OnDelete(DeleteBehavior.NoAction);

            // ---------------- Supervisor ↔ Replys ----------------
            modelBuilder.Entity<Supervisor>()
        .HasMany(s => s.Replys)
        .WithOne(r => r.Supervisor)
        .HasForeignKey(r => r.SupervisorId)
        .OnDelete(DeleteBehavior.NoAction); 

            // ---------------- Project ↔ Team (One-to-One) ----------------
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Project)
                .WithOne(p => p.Team)
                .HasForeignKey<Project>(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

//using GPMS.Models;
//using Microsoft.EntityFrameworkCore;

//namespace GPMS.Models
//{
//    public class AppDbContext : DbContext
//    {
//        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//        public DbSet<Student> Students { get; set; }
//        public DbSet<KanbanTask> Tasks { get; set; }
//        public DbSet<Link> Links { get; set; }
//        public DbSet<Feedback> Feedbacks { get; set; }
//        public DbSet<Team> Teams { get; set; }
//        public DbSet<Project> Projects { get; set; }
//        public DbSet<Supervisor> Supervisors { get; set; }
//        public DbSet<Reply> Replys { get; set; }
//        public DbSet<User> Users { get; set; } = null!;

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // ---------------- Primary Keys ----------------
//            modelBuilder.Entity<User>().HasKey(u => u.UserId);
//            modelBuilder.Entity<Student>().HasKey(s => s.UserId);
//            modelBuilder.Entity<Supervisor>().HasKey(s => s.UserId);
//            modelBuilder.Entity<Team>().HasKey(t => t.TeamId);
//            modelBuilder.Entity<KanbanTask>().HasKey(t => t.Id);
//            modelBuilder.Entity<Project>().HasKey(p => p.ProjectTitle);
//            modelBuilder.Entity<Feedback>().HasKey(f => f.Id);
//            modelBuilder.Entity<Link>().HasKey(l => l.Id);
//            modelBuilder.Entity<Reply>().HasKey(r => r.Id);

//            // ---------------- Identity settings ----------------
//            modelBuilder.Entity<User>().Property(u => u.UserId).ValueGeneratedNever();
//            modelBuilder.Entity<Student>().Property(s => s.UserId).ValueGeneratedNever();
//            modelBuilder.Entity<Supervisor>().Property(s => s.UserId).ValueGeneratedNever();
//            modelBuilder.Entity<Team>().Property(t => t.TeamId).ValueGeneratedNever();
//            modelBuilder.Entity<Project>().Property(p => p.Id).ValueGeneratedOnAdd(); // ProjectId auto increment

//            // ---------------- Team ----------------
//            modelBuilder.Entity<Team>()
//                .HasIndex(t => t.TeamName)
//                .IsUnique();

//            modelBuilder.Entity<Team>()
//                .HasMany(t => t.Students)
//                .WithOne(s => s.Team)
//                .HasForeignKey(s => s.TeamId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<Team>()
//                .HasMany(t => t.KanbanTasks)
//                .WithOne(t => t.Team)
//                .HasForeignKey(t => t.TeamId)
//                .OnDelete(DeleteBehavior.Restrict);

//            modelBuilder.Entity<Team>()
//                .HasOne(t => t.Project)
//                .WithOne(p => p.Team)
//                .HasForeignKey<Project>(p => p.TeamId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<Team>()
//                .HasOne(t => t.Supervisor)
//                .WithMany(s => s.Teams)
//                .HasForeignKey(t => t.SupervisorId)
//                .OnDelete(DeleteBehavior.SetNull);

//            // ---------------- Supervisor ----------------
//            modelBuilder.Entity<Supervisor>()
//                .HasMany(s => s.Feedbacks)
//                .WithOne(f => f.Supervisor)
//                .HasForeignKey(f => f.SupervisorId)
//                .OnDelete(DeleteBehavior.SetNull);

//            modelBuilder.Entity<Supervisor>()
//                .HasMany(s => s.Replys)
//                .WithOne(r => r.Supervisor)
//                .HasForeignKey(r => r.SupervisorId)
//                .OnDelete(DeleteBehavior.NoAction);

//            // ---------------- Student ----------------
//            modelBuilder.Entity<Student>()
//                .HasMany(s => s.Links)
//                .WithOne(l => l.Student)
//                .HasForeignKey(l => l.StudentId)
//                .OnDelete(DeleteBehavior.Restrict);

//            modelBuilder.Entity<Student>()
//                .HasMany(s => s.Replies)
//                .WithOne(r => r.Student)
//                .HasForeignKey(r => r.StudentId)
//                .OnDelete(DeleteBehavior.NoAction);

//            // ---------------- Link ----------------
//            modelBuilder.Entity<Link>()
//                .HasOne(l => l.Team)
//                .WithMany(t => t.Links)
//                .HasForeignKey(l => l.TeamId)
//                .OnDelete(DeleteBehavior.Restrict);

//            // ---------------- Feedback ----------------
//            modelBuilder.Entity<Feedback>()
//                 .HasOne(f => f.Team)
//                 .WithMany(t => t.Feedbacks) // Team يجب أن يحتوي على ICollection<Feedback> Feedbacks
//                 .HasForeignKey(f => f.TeamId)
//                 .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<Feedback>()
//                .HasMany(f => f.Replies)
//                .WithOne(r => r.Feedback)
//                .HasForeignKey(r => r.FeedbackId)
//                .OnDelete(DeleteBehavior.Cascade);

//            // ---------------- KanbanTask ----------------
//            modelBuilder.Entity<KanbanTask>()
//                .HasMany(k => k.AssignedStudents)
//                .WithMany(); // Many-to-many with Student

//            // ---------------- User ----------------
//            modelBuilder.Entity<User>()
//                .HasOne(u => u.Student)
//                .WithOne(s => s.User)
//                .HasForeignKey<Student>(s => s.UserId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<User>()
//                .HasOne(u => u.Supervisor)
//                .WithOne(sp => sp.User)
//                .HasForeignKey<Supervisor>(sp => sp.UserId)
//                .OnDelete(DeleteBehavior.Cascade);
//        }
//    }
//}



