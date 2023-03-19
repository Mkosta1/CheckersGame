using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Db;

public class GameRepositoryDb : BaseRepository, IGameRepository
{
    
    public GameRepositoryDb(AppDbContext dbContext) : base(dbContext)
    {
    }
    
    
    public List<CheckersGame> GetAll()
    {
        return Ctx.CheckerGame
            .Include(c => c.CheckersOption)
            .OrderBy(o => o.StartedAt)
            .ToList();
    }

    public CheckersGame? GetGame(int? id)
    {
        return Ctx.CheckerGame
            .Include(g => g.CheckersOption)
            .Include(g => g.CheckersGameState)
            .FirstOrDefault(g => g.Id == id);
    }

    public CheckersGame AddGame(CheckersGame game)
    {
        Ctx.CheckerGame.Add(game);
        Ctx.SaveChanges();
        return game;
    }
    

    public CheckersGame Update(CheckersGame game)
    {
        Ctx.CheckerGame.Update(game);
        Ctx.SaveChanges();
        return game;
    }

    public CheckersGame Delete(CheckersGame game)
    {
        Ctx.CheckerGame.Remove(game);
        Ctx.SaveChanges();
        return game;
    }
}