using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Db;

public class GameOptionsRepositoryDb : BaseRepository, IGameOptionsRepository
{
    public GameOptionsRepositoryDb(AppDbContext dbContext) : base(dbContext)
    {
    }

    public List<string> GetGameOptionsList()
    {
        // select name from options;
        var res    =    Ctx
            .CheckersOptions
            .Include(o => o.CheckerGames)
            .OrderBy(o => o.Name)
            .ToList();

        return res
            .Select(o => o.Name)
            .ToList();  

    }

    public CheckersOption GetGameOptions(string id)
    {
        return Ctx
            .CheckersOptions
            .First(o => o.Name == id);
    }

    public void SaveGameOptions(string id, CheckersOption option)
    {
        var optionsFromDb = Ctx.CheckersOptions.FirstOrDefault(o => o.Name == id);
        if (optionsFromDb == null)
        {
            Ctx.CheckersOptions.Add(option);
            Ctx.SaveChanges();
            return;
        }

        optionsFromDb.Name = option.Name;
        optionsFromDb.Width = option.Width;
        optionsFromDb.Height = option.Height;
        optionsFromDb.RandomMoves = option.RandomMoves;
        optionsFromDb.WhiteStart = option.WhiteStart;
        Ctx.SaveChanges();
    }

    public void DeleteGameOptions(string id)
    {

        var optionsFromDb = GetGameOptions(id);
        Ctx.CheckersOptions.Remove(optionsFromDb);
        Ctx.SaveChanges();
    }
    
}