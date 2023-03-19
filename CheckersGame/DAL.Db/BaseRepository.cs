namespace DAL.Db;

public abstract class BaseRepository : IBaseRepository
{
    protected readonly AppDbContext Ctx;

    protected BaseRepository(AppDbContext dbContext)
    {
        Ctx = dbContext;
    }

    public string Name { get; } = "DB";
    
    public void SaveChanged()
    {
        Ctx.SaveChanges();
    }
}