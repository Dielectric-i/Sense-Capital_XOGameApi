using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Interfaces
{
    public interface IPlayerRepository
    {
        // Create new Player
        Task<Player> CreatePlayerAsync(Player Player);
        
        // Get Player by id
        Task<Player> GetById(int id);

        // Get Player by Name
        Task<Player> GetByName(string name);

        // Get all Players
        Task<IEnumerable<Player>> GetAllPlayersAsync();

        //Delete Player by Id
        Task DeletePlayer(int id);

        //Update Player by Id
        Task<Player> Update(int id);
    }
}
