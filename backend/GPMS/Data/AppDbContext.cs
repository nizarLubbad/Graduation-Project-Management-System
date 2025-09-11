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
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Doing> Doings { get; set; }
        public DbSet<Done> Dones { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //add primary key for kan task
            //modelBuilder.Entity<KanbanTask>()
            //    .HasKey(t => t.Id);
            // MAKE ID NOT AUTO INCREMENT
            //modelBuilder.Entity<KanbanTask>()
            //    .Property(t => t.Id)
            //    .ValueGeneratedNever();
            ////////////////////////////////////////////////////////////////////////////
            // add primary key for student
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentId);
            // add primary key for supervisor
            modelBuilder.Entity<Supervisor>()
                .HasKey(s => s.SupervisorId);
            modelBuilder.Entity<Student>()
                .Property(s => s.StudentId)
                .ValueGeneratedNever();

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique();
            //configure cascade delete for student tasks when a student is deleted
            modelBuilder.Entity<Student>()
                .HasMany(s => s.StudentTask)
                .WithOne(st => st.Student)
                .HasForeignKey(st => st.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            //configure cascade delete for student tasks when a task is deleted
            modelBuilder.Entity<KanbanTask>()
                .HasMany(t => t.StudentTasks)
                .WithOne(st => st.Task)
                .HasForeignKey(st => st.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            //configure cascade delete for links when a task is deleted
            modelBuilder.Entity<KanbanTask>()
                .HasMany(t => t.Links)
                .WithOne(l => l.KanbanTask)
                .HasForeignKey(l => l.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            //configure cascade delete for feedbacks when a task is deleted
            modelBuilder.Entity<KanbanTask>()
                .HasMany(t => t.Feedbacks)
                .WithOne(f => f.KanbanTask)
                .HasForeignKey(f => f.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            //make email unique for supervisor
            modelBuilder.Entity<Supervisor>()
                .HasIndex(s => s.Email)
                .IsUnique();
            //make email unique for student
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique();
            //one-one relationship between todo and team
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Todo)
                .WithOne(te => te.Team)
                .HasForeignKey<Team>(te => te.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
            //one-one relationship between Doing and team
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Doing)
                .WithOne(te => te.Team)
                .HasForeignKey<Team>(te => te.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
            //one-one relationship between Done and team
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Done)
                .WithOne(te => te.Team)
                .HasForeignKey<Team>(te => te.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
            // One-to-Many: Supervisor -> Teams
            modelBuilder.Entity<Supervisor>()
                .HasMany(s => s.Teams)
                .WithOne(t => t.Supervisor)
                .HasForeignKey(t => t.SupervisorId);
            //one to one relationship between project and team
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Team)
                .WithOne(t => t.Project)
                .HasForeignKey<Team>(t => t.ProjectTitle)
                .OnDelete(DeleteBehavior.Cascade);




            //one to one relationship between student and team
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Team)
                .WithOne(t => t.Student)
                .HasForeignKey<Team>(t => t.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Student>()
            //    .Property(s => s.Password)
            //    .HasColumnType("varchar(100)");

            modelBuilder.Entity<StudentTask>()
                .HasKey(st => new { st.StudentId, st.TaskId }); // add composite primary key

            modelBuilder.Entity<StudentTask>()
                .HasOne(st => st.Student)
                .WithMany(s => s.StudentTask)
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
            //make project id auto increment
            modelBuilder.Entity<Project>()
                .Property(p => p.ProjectId)
                .ValueGeneratedOnAdd();

        }
    }
}
