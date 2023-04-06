 using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sense_Capital_XOGameApi.Data;
using Sense_Capital_XOGameApi.Interfaces;
using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApiContext _context;
        private readonly IValidator<Game> _gameInitialStateValidator;

        public GameRepository(ApiContext context, IValidator<Game> gameInitialStateValidator)
        {
            _gameInitialStateValidator = gameInitialStateValidator;
            _context = context;
        }

        // Create new game
        public async Task<Game> CreateGameAsync(Game game)
        {
            try
            {
                var validationResult = await _gameInitialStateValidator.ValidateAsync(game);
                if (!validationResult.IsValid)
                    throw new Exception("The 'game' parameter is not valid: " + validationResult.ToString(", "));


                await _context.Games.AddAsync(game);
                await _context.SaveChangesAsync();
                game = await GetGameByIdAsync(game.Id);
                return game;
            }
            catch (Exception ex)
            {
                //Something went wrong while creating the game.
                throw new Exception("GameRepository API: An error occurred in the CreateGameAsync method: " + ex.Message);
            }
        }

        // Get all games
        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            try
            {
                var games = await _context.Games
                                .Include(g => g.Players)
                                .Include(g => g.Moves)
                                .ToListAsync();
                return games;
            }
            catch (Exception ex)
            {

                throw new Exception("GameRepository API: An error occurred in the GetAllGamesAsync method: " + ex.Message);
            }
        }

        // Delete all games
        public async Task DeleteAllGamesAsync()
        {
            try
            {
                var allGames = await _context.Games.ToListAsync();
                _context.Games.RemoveRange(allGames);
                // await _context.Games.ExecuteDeleteAsync(); // этот метод генерирует неправильный SQL запрос
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("GameRepository API: An error occurred in the DeleteAllGamesAsync method: " + ex.Message);
            }
        }

        // Get game by id
        public async Task<Game> GetGameByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException($"Invalid game id: {id}");


                var game = await _context.Games
                                .Include(g => g.Players)
                                .Include(g => g.Moves)
                                .FirstOrDefaultAsync(g => g.Id == id);

                return game;
            }
            catch (Exception ex)
            {
                throw new Exception("GameRepository API: An error occurred in the GetGameByIdAsync method: " + ex.Message);
            }
        }

        // Delete game by Id
        public async Task DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException($"Invalid game id: {id}");

                var game = await GetGameByIdAsync(id);
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("GameRepository API: An error occurred in the DeleteAsync method: " + ex.Message);
            }
        }

        // Update game
        public async Task<Game> UpdateGameAsync(Game game)
        {
            try
            {
                var validationResult = await _gameInitialStateValidator.ValidateAsync(game);
                if (!validationResult.IsValid)
                    throw new ArgumentException("The 'game' parameter is not valid: " + validationResult.ToString(", "));


                _context.Games.Update(game);
                await _context.SaveChangesAsync();
                return game;
            }
            catch (Exception ex)
            {

                throw new Exception("GameRepository API: An error occurred in the UpdateGameAsync method: " + ex.Message);
            }
        }
    }
}