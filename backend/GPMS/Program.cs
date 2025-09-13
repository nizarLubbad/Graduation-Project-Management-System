using GPMS.Middlewares;
using GPMS.Models;
using GPMS.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
