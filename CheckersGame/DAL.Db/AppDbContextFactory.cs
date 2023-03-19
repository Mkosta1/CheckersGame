using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace DAL.Db;



public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=C:/Users/marku/RiderProjects/icd0008-2022f/CheckersGame/identifier.sqlite");

        return new AppDbContext(optionsBuilder.Options);
    }
}