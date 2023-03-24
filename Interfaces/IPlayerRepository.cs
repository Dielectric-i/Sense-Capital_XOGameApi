using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Interfaces
{
    public interface IPlayerRepository
    {
        // Create new Player
        Task<Player> CreatePlayerAsync(Player Player);

        // Get Player by Name
        Task<Player> GetByName(string name);

        // Checking for the existence of an player
        Task<bool> isPlayerExist(int id);
    }
}
