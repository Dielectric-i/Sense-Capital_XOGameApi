using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Sense_Capital_XOGameApi.Data;
using Sense_Capital_XOGameApi.Interfaces;
using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Repositories
{
    public class MoveRepository : IMoveRepository
    {
        private readonly ApiContext _context;

        public MoveRepository(ApiContext context)
        {
            _context = context;
        }

        // Create new Move
        public async Task<Move> CreateMoveAsync(Move move)
        {
            if(move == null)
            {
                throw new ArgumentNullException($"No Moves found");
            }
            _context.Moves.Add(move);
            await _context.SaveChangesAsync();
            return Move;
        }

        // Get Move by id
        public async Task<Move> GetById(int id)
        {
            var player = await _context.Moves.FindAsync(id);
            if (player == null)
            {
                throw new ArgumentException($"Move with id {id} not found.");
            }
            return player;
        }

        // Get Move by Name
        public async Task<Move> GetByName(string name)
        {

            var player = await _context.Moves.FirstOrDefaultAsync(u => u.Name == name);
            return player;
        }

        // Get all Moves
        public async Task<IEnumerable<Move>> GetAllMovesAsync()
        {
            var Moves = await _context.Moves.ToListAsync();
            return Moves;
        }

        //Delete Move by Id
        public async Task DeleteMove(int id)
        {
            var Move = await GetById(id);
            _context.Moves.Remove(Move);
            await _context.SaveChangesAsync();
        }

        public async Task<Move> Update(int id)
        {
            var Move = await GetById(id);
            _context.Moves.Update(Move);
            return Move;
        }
    }
}