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
            var validationResult = await _gameInitialStateValidator.ValidateAsync(game);
            if (!validationResult.IsValid)
                throw new Exception("GameRepository API: The 'game' parameter is not valid: " + validationResult.ToString(", "));

            try
            {
                await _context.Games.AddAsync(game);
                await _context.SaveChangesAsync();
                game = await GetGameByIdAsync(game.Id);
                return game;
            }
            catch (Exception ex)
            {
                //Something went wrong while creating the game.
                throw new Exception("GameRepository API: Error adding gamе: " + ex.Message);
            }
        }

        // Get game by id
        public async Task<Game> GetGameByIdAsync(int id)
        {
            if (id <= 0 )
                throw new ArgumentException($"GameRepository API: Invalid game id: {id}");

            try
            {
                var game = await _context.Games
                                .Include(g => g.Players)
                                .Include(g => g.Moves)
                                .FirstOrDefaultAsync(g => g.Id == id);
                return game;
            }
            catch (Exception ex)
            {
                throw new Exception("GameRepository API: Error getting game by id: " + ex.Message);
            }
        }

        // Get all games
        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            try
            {
                var games = await _context.Games.ToListAsync();
                return games;
            }
            catch (Exception ex)
            {

                throw new Exception("Repository API: Error getting list of all games: " + ex.Message);
            }
        }

        //Delete game by Id
        public async Task DeleteAsync(int id)
        {
            if (id <= 0 || id == null)
                throw new ArgumentException($"GameRepository API: Invalid game id: {id}");

            try
            {
                var game = await GetGameByIdAsync(id);
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("GameRepository API: Error deleting game by id: " + ex.Message);
            }
        }

        public async Task<Game> UpdateGameAsync(Game game)
        {
            var validationResult = await _gameInitialStateValidator.ValidateAsync(game);
            if (!validationResult.IsValid)
                throw new Exception("GameRepository API: The game parameter is not valid: " + validationResult.ToString(", "));

            try
            {
                _context.Games.Update(game);
                _context.SaveChanges();
                return game;
            }
            catch (Exception ex)
            {

                throw new Exception("GameRepository API: Error updating game by id: " + ex.Message);
            }
        }
    }
}