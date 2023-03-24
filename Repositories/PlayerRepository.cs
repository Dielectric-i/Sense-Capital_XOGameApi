using Microsoft.EntityFrameworkCore;
using Sense_Capital_XOGameApi.Data;
using Sense_Capital_XOGameApi.Interfaces;
using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ApiContext _context;

        public PlayerRepository(ApiContext context)
        {
            _context = context;
        }

        // Create new Player
        public async Task<Player> CreatePlayerAsync(Player Player)
        {
            if(Player == null)
            {
                throw new ArgumentNullException($"No Players found");
            }
            _context.Players.Add(Player);
            await _context.SaveChangesAsync();
            return Player;
        }

        // Get Player by Name
        public async Task<Player> GetByName(string name)
        {

            var player = await _context.Players.FirstOrDefaultAsync(u => u.Name == name);
            return player;
        }

        // Checking for the existence of an player
        public async Task<bool> isPlayerExist(int id)
        {
            if (!int.TryParse(id.ToString(), out int parsedId))
            {
                throw new ArgumentException($"Invalid player id: {id}");
            }
            return await _context.Players.AnyAsync(p => p.Id == id);
        }

    }
}