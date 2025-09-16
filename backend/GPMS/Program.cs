using GPMS.Middlewares;
using GPMS.Models;
using GPMS.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
//add authentication
builder.Services.AddScoped<AuthService>();

var jwtKey = builder.Configuration["Jwt:Key"] 
             ?? throw new InvalidOperationException("JWT Key not configured.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] 
                ?? throw new InvalidOperationException("JWT Issuer not configured.");
var jwtAudience = builder.Configuration["Jwt:Audience"] 
                  ?? throw new InvalidOperationException("JWT Audience not configured.");

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//make the connection string singleton
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found.");
builder.Services.AddSingleton(new ConnectionStringProvider(connectionString));

builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    var connProvider = sp.GetRequiredService<ConnectionStringProvider>();
    options.UseSqlServer(connProvider.ConnectionString);
});
//end
var app = builder.Build();

// Logging and Exception Handling Middlewares
app.UseMiddleware<LoggingMiddleware>();           
app.UseMiddleware<ExceptionHandlingMiddleware>();
// End Logging and Exception Handling Middlewares



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
