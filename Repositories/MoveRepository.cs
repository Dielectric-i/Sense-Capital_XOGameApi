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

        public async Task<Move> CreateMoveAsync(Move move)
        {
            try
            {
                await _context.Moves.AddAsync(move);
                await _context.SaveChangesAsync();
                return move;
            }
            catch (Exception ex)
            {
                throw new Exception("MoveRepository API: Error adding move: " + ex.Message);
            }
        }
    }
}