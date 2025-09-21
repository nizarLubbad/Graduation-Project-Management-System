//using GPMS.Helpers;
//using GPMS.Interfaces;
//using GPMS.Middlewares;
//using GPMS.Models;
//using GPMS.Repositories;
//using GPMS.Services;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//namespace GPMS
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // ---------------- Add services to the container ----------------

//            builder.Services.AddControllers();
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            // ---------- Repositories ----------
//            builder.Services.AddScoped<IUserRepository, UserRepository>();
//            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
//            builder.Services.AddScoped<ISupervisorRepository, SupervisorRepository>();
//            builder.Services.AddScoped<ITeamRepository, TeamRepository>();
//            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
//            builder.Services.AddScoped<TaskRepository, TaskRepository>();
//            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
//            builder.Services.AddScoped<ILinkRepository, LinkRepository>();
//            builder.Services.AddScoped<IReplyRepository, ReplyRepository>();
//            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
//            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
//            builder.Services.AddScoped<IBaseRepository<KanbanTask>, BaseRepository<KanbanTask>>();

//            // ---------- Services ----------
//            builder.Services.AddScoped<IAuthService, AuthService>();
//            builder.Services.AddScoped<IStudentService, StudentService>();
//            builder.Services.AddScoped<ISupervisorService, SupervisorService>();
//            builder.Services.AddScoped<ITeamService, TeamService>();
//            builder.Services.AddScoped<IProjectService, ProjectService>();
//            builder.Services.AddScoped<IKanbanTaskService, KanbanTaskService>();
//            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
//            builder.Services.AddScoped<ILinkService, LinkService>();
//            builder.Services.AddScoped<IReplyService, ReplyService>();

//            // ---------- Helpers ----------
//            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
//            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//            // ---------- JWT Authentication ----------
//            var jwtSettings = builder.Configuration.GetSection("Jwt");
//            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

//            builder.Services.AddAuthentication(options =>
//            {
//                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            })
//            .AddJwtBearer(options =>
//            {
//                options.RequireHttpsMetadata = false;
//                options.SaveToken = true;
//                options.TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidateLifetime = true,
//                    ValidateIssuerSigningKey = true,
//                    ValidIssuer = jwtSettings["Issuer"],
//                    ValidAudience = jwtSettings["Audience"],
//                    IssuerSigningKey = new SymmetricSecurityKey(key)
//                };
//            });

//            // ---------- Database ----------
//            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
//                ?? throw new InvalidOperationException("Connection string not found.");

//            builder.Services.AddSingleton(new ConnectionStringProvider(connectionString));

//            builder.Services.AddDbContext<AppDbContext>((sp, options) =>
//            {
//                var connProvider = sp.GetRequiredService<ConnectionStringProvider>();
//                options.UseSqlServer(connProvider.ConnectionString);
//            });

//            // ---------------- Build App ----------------
//            var app = builder.Build();

//            // ---------------- Middlewares ----------------
//            app.UseMiddleware<LoggingMiddleware>();
//            app.UseMiddleware<ExceptionHandlingMiddleware>();

//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();
//            app.UseAuthentication();
//            app.UseAuthorization();
//            app.MapControllers();

//            app.Run();
//        }
//    }
//}
using GPMS.Helpers;
using GPMS.Interfaces;
using GPMS.Middlewares;
using GPMS.Models;
using GPMS.Repositories;
using GPMS.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GPMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------------- Add services ----------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ---------------- Repositories ----------------
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ISupervisorRepository, SupervisorRepository>();
            builder.Services.AddScoped<ITeamRepository, TeamRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<TaskRepository, TaskRepository>();
            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddScoped<ILinkRepository, LinkRepository>();
            builder.Services.AddScoped<IReplyRepository, ReplyRepository>();
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<IBaseRepository<KanbanTask>, BaseRepository<KanbanTask>>();

            // ---------------- Services ----------------
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ISupervisorService, SupervisorService>();
            builder.Services.AddScoped<ITeamService, TeamService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IKanbanTaskService, KanbanTaskService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<ILinkService, LinkService>();
            builder.Services.AddScoped<IReplyService, ReplyService>();

            // ---------------- Helpers ----------------
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            // ---------------- AutoMapper ----------------
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // ---------------- JWT Authentication ----------------
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // ---------------- Database ----------------
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");

            builder.Services.AddSingleton(new ConnectionStringProvider(connectionString));

            builder.Services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var connProvider = sp.GetRequiredService<ConnectionStringProvider>();
                options.UseSqlServer(connProvider.ConnectionString);
            });

            // ---------------- Build App ----------------
            var app = builder.Build();

            // ---------------- Middlewares ----------------
            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

       //     if (app.Environment.IsDevelopment())
       //     {
                app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty; // يخلي Swagger هو الصفحة الرئيسية
            });
            //     }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}


