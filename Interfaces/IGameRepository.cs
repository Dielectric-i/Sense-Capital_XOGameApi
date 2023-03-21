using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Interfaces
{
    public interface IGameRepository
    {
        // Create new game
        Task<Game> CreateGameAsync(Game game);
        
        // Get game by id
        Task<Game> GetGameAsync(int id);

        // Get all games
        Task<IEnumerable<Game>> GetAllGamesAsync();

        //Delete game by Id
        Task DeleteAsync(int id);

        //Update game by Id
        Task<Game> UpdateAsync(Game game);
    }
}
