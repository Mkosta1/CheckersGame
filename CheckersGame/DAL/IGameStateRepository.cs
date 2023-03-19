using Domain;

namespace DAL;

public interface IGameStateRepository : IBaseRepository
{
    void AddState(CheckersGameState state);
    void GetState(int id);
    // void GetLatestStateForGame(int gameId);
    
}