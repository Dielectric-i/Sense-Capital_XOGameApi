using Microsoft.AspNetCore.Http.HttpResults;
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

        // Get Player by id
        public async Task<Player> GetById(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                throw new ArgumentException($"Player with id {id} not found.");
            }
            return player;
        }

        // Get Player by Name
        public async Task<Player> GetByName(string name)
        {

            var player = await _context.Players.FirstOrDefaultAsync(u => u.Name == name);
            return player;
        }

        // Get all Players
        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            var Players = await _context.Players.ToListAsync();
            return Players;
        }

        //Delete Player by Id
        public async Task DeletePlayer(int id)
        {
            var Player = await GetById(id);
            _context.Players.Remove(Player);
            await _context.SaveChangesAsync();
        }

        public async Task<Player> Update(int id)
        {
            var Player = await GetById(id);
            _context.Players.Update(Player);
            return Player;
        }
    }
}