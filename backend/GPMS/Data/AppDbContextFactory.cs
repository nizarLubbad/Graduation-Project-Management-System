using GPMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GPMS.Models
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer("Server=SQL6032.site4now.net,1433;Database=db_abe02b_backendteam;User Id=db_abe02b_backendteam_admin;Password=backendTeam@2025;Encrypt=True;TrustServerCertificate=False;MultipleActiveResultSets=True;Persist Security Info=False;");


            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

