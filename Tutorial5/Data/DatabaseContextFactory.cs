using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Tutorial5.Data;

namespace Tutorial5.Data;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=Tutorial5Db;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;");
        return new DatabaseContext(optionsBuilder.Options);
    }
}