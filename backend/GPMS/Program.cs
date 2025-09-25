
using GPMS.Helpers;
using GPMS.Interfaces;
using GPMS.Middlewares;
using GPMS.Models;
using GPMS.Repositories;
using GPMS.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace GPMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // ---------------- Add services ----------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("SecureCORS", policy =>
                {
                    policy.SetIsOriginAllowed(origin =>
                        origin == "https://backendteam-001-site1.qtempurl.com" ||
                        origin.StartsWith("https://localhost") ||
                        origin.StartsWith("http://localhost"))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

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
            builder.Services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            builder.Services.AddScoped<IBaseRepository<KanbanTask, long>, BaseRepository<KanbanTask, long>>();

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
            builder.Services.AddScoped<TokenTestService>();


            // ---------------- Helpers ----------------
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            // ---------------- AutoMapper ----------------
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



            // ---------------- Database ----------------
            var connectionString = builder.Configuration.GetConnectionString("CloudConnection")
                ?? throw new InvalidOperationException("Connection string not found.");

            builder.Services.AddSingleton(new ConnectionStringProvider(connectionString));

            builder.Services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var connProvider = sp.GetRequiredService<ConnectionStringProvider>();
                options.UseSqlServer(connProvider.ConnectionString);
            });

            Console.WriteLine("Issuer: " + builder.Configuration["Jwt:Issuer"]);
            Console.WriteLine("Audience: " + builder.Configuration["Jwt:Audience"]);
            Console.WriteLine("Key: " + builder.Configuration["Jwt:Key"]);
            ///////////////////////////////////////////////////////////////////
            ///
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
                    ValidAudience = builder.Configuration["JwtConfig:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,




                };
            });

            builder.Services.AddAuthorization();

            // ---------------- Build App ----------------
            var app = builder.Build();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // ---------------- Middlewares ----------------


            app.UseCors("SecureCORS");


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GPMS API V1");
            });
            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
//using GPMS.Helpers;
//using GPMS.Interfaces;
//using GPMS.Middlewares;
//using GPMS.Models;
//using GPMS.Repositories;
//using GPMS.Services;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.HttpOverrides;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using System.Security.Claims;
//using System.Text;
//using Serilog; // إضافة Serilog للتسجيل المتقدم

//namespace GPMS
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            // تكوين Serilog للتسجيل المتقدم
//            Log.Logger = new LoggerConfiguration()
//                .WriteTo.Console()
//                .WriteTo.File("logs/gpms-log-.txt", rollingInterval: RollingInterval.Day)
//                .CreateLogger();

//            try
//            {
//                Log.Information("Starting GPMS application...");

//                var builder = WebApplication.CreateBuilder(args);
//                var config = builder.Configuration;
//                var environment = builder.Environment;

//                // إضافة Serilog إلى builder
//                builder.Host.UseSerilog();

//                // إعدادات خاصة بالبيئة
//                Log.Information("Environment: {Environment}", environment.EnvironmentName);
//                Log.Information("Application Name: {ApplicationName}", environment.ApplicationName);
//                Log.Information("Content Root Path: {ContentRootPath}", environment.ContentRootPath);

//                // ---------------- Add services ----------------
//                builder.Services.AddControllers();
//                builder.Services.AddEndpointsApiExplorer();

//                // تكوين Swagger بشكل مختلف حسب البيئة
//                builder.Services.AddSwaggerGen(c =>
//                {
//                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//                    {
//                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
//                        Name = "Authorization",
//                        In = ParameterLocation.Header,
//                        Type = SecuritySchemeType.Http,
//                        Scheme = "Bearer"
//                    });

//                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//                    {
//                        {
//                            new OpenApiSecurityScheme
//                            {
//                                Reference = new OpenApiReference
//                                {
//                                    Type = ReferenceType.SecurityScheme,
//                                    Id = "Bearer"
//                                }
//                            },
//                            new string[] {}
//                        }
//                    });

//                    // إضافة معلومات إضافية في Development
//                    if (environment.IsDevelopment())
//                    {
//                        c.SwaggerDoc("v1", new OpenApiInfo
//                        {
//                            Title = "GPMS API - Development",
//                            Version = "v1",
//                            Description = "Development Environment - Debug Mode"
//                        });
//                    }
//                    else
//                    {
//                        c.SwaggerDoc("v1", new OpenApiInfo
//                        {
//                            Title = "GPMS API",
//                            Version = "v1",
//                            Description = "Production Environment"
//                        });
//                    }
//                });

//                // تكوين CORS بشكل مختلف حسب البيئة
//                if (environment.IsDevelopment())
//                {
//                    builder.Services.AddCors(options =>
//                    {
//                        options.AddPolicy("DevelopmentCORS", policy =>
//                        {
//                            policy.AllowAnyOrigin()
//                                  .AllowAnyHeader()
//                                  .AllowAnyMethod();
//                        });
//                    });
//                }
//                else
//                {
//                    builder.Services.AddCors(options =>
//                    {
//                        options.AddPolicy("ProductionCORS", policy =>
//                        {
//                            policy.WithOrigins(
//                                "https://backendteam-001-site1.qtempurl.com",
//                                "https://your-production-domain.com")
//                            .AllowAnyHeader()
//                            .AllowAnyMethod()
//                            .AllowCredentials();
//                        });
//                    });
//                }

//                // ---------------- Repositories ----------------
//                builder.Services.AddScoped<IUserRepository, UserRepository>();
//                builder.Services.AddScoped<IStudentRepository, StudentRepository>();
//                builder.Services.AddScoped<ISupervisorRepository, SupervisorRepository>();
//                builder.Services.AddScoped<ITeamRepository, TeamRepository>();
//                builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
//                builder.Services.AddScoped<TaskRepository, TaskRepository>();
//                builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
//                builder.Services.AddScoped<ILinkRepository, LinkRepository>();
//                builder.Services.AddScoped<IReplyRepository, ReplyRepository>();
//                builder.Services.AddScoped<IJwtProvider, JwtProvider>();
//                builder.Services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
//                builder.Services.AddScoped<IBaseRepository<KanbanTask, long>, BaseRepository<KanbanTask, long>>();

//                // ---------------- Services ----------------
//                builder.Services.AddScoped<IAuthService, AuthService>();
//                builder.Services.AddScoped<IStudentService, StudentService>();
//                builder.Services.AddScoped<ISupervisorService, SupervisorService>();
//                builder.Services.AddScoped<ITeamService, TeamService>();
//                builder.Services.AddScoped<IProjectService, ProjectService>();
//                builder.Services.AddScoped<IKanbanTaskService, KanbanTaskService>();
//                builder.Services.AddScoped<IFeedbackService, FeedbackService>();
//                builder.Services.AddScoped<ILinkService, LinkService>();
//                builder.Services.AddScoped<IReplyService, ReplyService>();
//                builder.Services.AddScoped<TokenTestService>();

//                // ---------------- Helpers ----------------
//                builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

//                // ---------------- AutoMapper ----------------
//                builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//                // ---------------- Database ----------------
//                var connectionString = builder.Configuration.GetConnectionString("CloudConnection")
//                    ?? throw new InvalidOperationException("Connection string not found.");

//                builder.Services.AddSingleton(new ConnectionStringProvider(connectionString));

//                builder.Services.AddDbContext<AppDbContext>((sp, options) =>
//                {
//                    var connProvider = sp.GetRequiredService<ConnectionStringProvider>();
//                    options.UseSqlServer(connProvider.ConnectionString);

//                    // تفعيل التسجيل التفصيلي في Development فقط
//                    if (environment.IsDevelopment())
//                    {
//                        options.EnableSensitiveDataLogging();
//                        options.EnableDetailedErrors();
//                    }
//                });

//                if (environment.IsDevelopment())
//                {
//                    Log.Debug("=== Development Environment Settings ===");
//                    Log.Debug("Issuer: {Issuer}", builder.Configuration["JwtConfig:Issuer"]);
//                    Log.Debug("Audience: {Audience}", builder.Configuration["JwtConfig:Audience"]);
//                    Log.Debug("Key: {Key}", builder.Configuration["JwtConfig:Key"]);
//                    Log.Debug("Connection String: {ConnectionString}", connectionString);
//                }

//                // ---------------- JWT Authentication مع تسجيل مفصل ----------------
//                var jwtKey = builder.Configuration["JwtConfig:Key"];
//                if (string.IsNullOrEmpty(jwtKey))
//                {
//                    Log.Error("JWT Key is not configured");
//                    throw new InvalidOperationException("JWT Key is not configured");
//                }

//                var jwtIssuer = builder.Configuration["JwtConfig:Issuer"];
//                var jwtAudience = builder.Configuration["JwtConfig:Audience"];

//                Log.Information("JWT Configuration - Issuer: {Issuer}, Audience: {Audience}", jwtIssuer, jwtAudience);

//                builder.Services.AddAuthentication(options =>
//                {
//                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//                }).AddJwtBearer(options =>
//                {
//                    // تفعيل HTTPS في Production فقط
//                    options.RequireHttpsMetadata = !environment.IsDevelopment();
//                    options.SaveToken = true;

//                    // إضافة event handlers للتسجيل المفصل
//                    options.Events = new JwtBearerEvents
//                    {
//                        OnAuthenticationFailed = context =>
//                        {
//                            Log.Warning("JWT Authentication Failed: {ErrorMessage}", context.Exception.Message);
//                            Log.Debug("Authentication failure details: {Exception}", context.Exception.ToString());
//                            return Task.CompletedTask;
//                        },
//                        OnChallenge = context =>
//                        {
//                            Log.Warning("JWT Challenge: {Error}, {Description}", context.Error, context.ErrorDescription);
//                            Log.Debug("Challenge details - Request Path: {Path}, Headers: {Headers}",
//                                context.Request.Path,
//                                string.Join("; ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}")));
//                            return Task.CompletedTask;
//                        },
//                        OnTokenValidated = context =>
//                        {
//                            Log.Information("JWT Token validated successfully for user: {User}",
//                                context.Principal?.Identity?.Name);
//                            return Task.CompletedTask;
//                        },
//                        OnMessageReceived = context =>
//                        {
//                            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//                            Log.Debug("JWT Token received: {TokenLength} characters", token?.Length ?? 0);
//                            return Task.CompletedTask;
//                        }
//                    };

//                    options.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        ValidIssuer = jwtIssuer,
//                        ValidAudience = jwtAudience,
//                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
//                        ValidateIssuer = true,
//                        ValidateAudience = true,
//                        ValidateLifetime = true,
//                        ValidateIssuerSigningKey = true,
//                        ClockSkew = TimeSpan.Zero // لا توجد فترة سماح للتحقق من الصلاحية
//                    };
//                });

//                builder.Services.AddAuthorization(options =>
//                {
//                    // تسجيل محاولات الوصول إلى السياسات
//                    options.AddPolicy("RequireAuthenticated", policy =>
//                    {
//                        policy.RequireAuthenticatedUser();
//                    });
//                });

//                // إضافة خدمات التسجيل
//                builder.Services.AddLogging(loggingBuilder =>
//                {
//                    loggingBuilder.AddConsole();
//                    loggingBuilder.AddDebug();
//                    loggingBuilder.AddSerilog();
//                });

//                // ---------------- Build App ----------------
//                var app = builder.Build();

//                // ---------------- Middleware Configuration ----------------
//                app.UseForwardedHeaders(new ForwardedHeadersOptions
//                {
//                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
//                });

//                // استخدام CORS المناسب للبيئة
//                if (environment.IsDevelopment())
//                {
//                    app.UseCors("DevelopmentCORS");
//                }
//                else
//                {
//                    app.UseCors("ProductionCORS");
//                }

//                // تكوين Swagger بشكل مختلف
//                if (environment.IsDevelopment())
//                {
//                    // في Development: Swagger متاح دائمًا
//                    app.UseSwagger();
//                    app.UseSwaggerUI(c =>
//                    {
//                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GPMS API V1 - Development");
//                        c.RoutePrefix = "swagger";
//                    });
//                }
//                else
//                {
//                    // في Production: يمكنك تعطيل Swagger أو حمايته بكلمة مرور
//                    app.UseSwagger();
//                    app.UseSwaggerUI(c =>
//                    {
//                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GPMS API V1");
//                        c.RoutePrefix = string.Empty;
//                    });
//                }

//                // HTTPS Redirection (مهم في Production)
//                if (!environment.IsDevelopment())
//                {
//                    app.UseHttpsRedirection();
//                }

//                // Middleware مخصص لتسجيل طلبات الـ 401
//                app.Use(async (context, next) =>
//                {
//                    await next();

//                    if (context.Response.StatusCode == 401)
//                    {
//                        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

//                        logger.LogWarning("401 Unauthorized - Path: {Path}, Method: {Method}, User: {User}, Headers: {Headers}",
//                            context.Request.Path,
//                            context.Request.Method,
//                            context.User?.Identity?.Name ?? "Anonymous",
//                            string.Join("; ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}")));

//                        // تسجيل تفاصيل الـ JWT token إذا كان موجودًا
//                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
//                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
//                        {
//                            var token = authHeader.Substring("Bearer ".Length).Trim();
//                            logger.LogDebug("JWT Token in 401 request: {TokenPrefix}... (Length: {TokenLength})",
//                                token.Substring(0, Math.Min(20, token.Length)),
//                                token.Length);
//                        }
//                    }
//                });

//                app.UseAuthentication();
//                app.UseAuthorization();

//                // Middlewares الإضافية
//                app.UseMiddleware<LoggingMiddleware>();
//                app.UseMiddleware<ExceptionHandlingMiddleware>();

//                app.MapControllers();

//                Log.Information("GPMS application started successfully in {Environment} mode", environment.EnvironmentName);
//                app.Run();
//            }
//            catch (Exception ex)
//            {
//                Log.Fatal(ex, "Application terminated unexpectedly");
//                throw;
//            }
//            finally
//            {
//                Log.CloseAndFlush();
//            }
//        }
//    }
//}
