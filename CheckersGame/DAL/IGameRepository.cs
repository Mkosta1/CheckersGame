using Domain;

namespace DAL;

public interface IGameRepository : IBaseRepository
{
    //crud- create, read, update, delete
    List<CheckersGame> GetAll();
    CheckersGame? GetGame(int? id);
    CheckersGame AddGame(CheckersGame game);

    CheckersGame Update(CheckersGame game);

    CheckersGame Delete(CheckersGame id);


}