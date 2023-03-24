using Microsoft.AspNetCore.Mvc;
using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Interfaces
{
    public interface IGameRepository
    {
        // Create new game
        Task<Game> CreateGameAsync(Game game);
        
        // Get all games
        Task<IEnumerable<Game>> GetAllGamesAsync();

        // Delete all games
        Task DeleteAllGamesAsync();

        // Get game by id
        Task<Game> GetGameByIdAsync(int id);

        //Delete game by Id
        Task DeleteAsync(int id);

        //Update game by Id
        Task<Game> UpdateGameAsync(Game game);
    }
}
