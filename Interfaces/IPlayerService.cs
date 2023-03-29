using Microsoft.AspNetCore.Mvc;
using Sense_Capital_XOGameApi.Models;
using Sense_Capital_XOGameApi.RequestModels;

namespace Sense_Capital_XOGameApi.Interfaces
{
    public interface IPlayerService
    {
        Task<ActionResult<Player>> CreatePlayerAsync(RqstCreatePlayer rqstCreatePlayer);
        Task<ActionResult<Player>> GetPlayerByIdAsync(int id);
    }
}
