using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Interfaces
{
    public interface IPlayerRepository
    {
        // Create new Player
        Task<Player> CreatePlayerAsync(Player Player);

        // Get Player by Id
        Task<Player> GetPlayerByIdAsync(int id);

        Task DeletePlayerByIdAsync(int id);

        // Get Player by Name
        Task<Player> GetPlayerByNameAsync(string name);

        // Checking for the existence of an player
        Task<bool> isPlayerExistById(int id);
        Task<bool> isPlayerExistByName(string name);
    }
}
