using Microsoft.AspNetCore.Mvc;
using Sense_Capital_XOGameApi.Models;
using Sense_Capital_XOGameApi.RequestModels;

namespace Sense_Capital_XOGameApi.Controllers
{
    public interface IGameService
    {
        Task<ActionResult<Game>> CreateGameAsync(RqstCreateGame rqstCreateGame);
        Task<ActionResult<IEnumerable<Game>>> GetAllGamesAsync();
        Task<ActionResult> DeleteAllGamesAsync();
        Task<ActionResult<Game>> GetGameAsync(int id);
        Task<ActionResult> DeleteGameAsync(int id);
        Task<ActionResult<Game>> MakeMoveAsync(RqstMakeMove rqstMakeMove);
    }
}
