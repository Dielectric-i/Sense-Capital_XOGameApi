using Microsoft.EntityFrameworkCore;
using Sense_Capital_XOGameApi.Data;
using Sense_Capital_XOGameApi.Interfaces;
using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApiContext _context;

        public GameRepository(ApiContext context)
        {
            _context = context;
        }

        // Create new game
        public async Task<Game> CreateGameAsync(Game game)
        {
                if (game == null)
                {
                    throw new ArgumentNullException($"The game parameter is null");
                }
                await _context.Games.AddAsync(game);
                await _context.SaveChangesAsync();
                return game;
        }

        // Get game by id
        public async Task<Game> GetGameAsync(int id)
        {
                var game = await _context.Games.FindAsync(id);
                if (game == null)
                {
                    throw new ArgumentException($"Game with id {id} not found.");
                }
                return game;
        }

        // Get all games
        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
                var games = await _context.Games.ToListAsync();
                return games;
        }

        //Delete game by Id
        public async Task DeleteAsync(int id)
        {
                var game = await GetGameAsync(id);
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
        }

        public async Task<Game> UpdateAsync(int id)
        {
                var game = await GetGameAsync(id);
                _context.Games.Update(game);
                return game;
        }
    }
}