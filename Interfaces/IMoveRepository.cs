using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Interfaces
{
    public interface IMoveRepository
    {
        // Create new Move
        Task<Move> CreateMoveAsync(Move Move);
        
        // Get Move by id
        Task<Move> GetByIdAsync(int id);

        // Get Move by Game Id
        Task<IEnumerable<Move>> GetByGameIdAsync(int gameId);

        // Get Move by Player Id
        Task<IEnumerable<Move>> GetByPlayerIdAsync(int PlayerId);

        //Delete Move by Id
        Task DeleteMoveByIdAsync(int id);

        //Update Move by Id
        //Task<Move> UpdateMoveById(int id);
    }
}
