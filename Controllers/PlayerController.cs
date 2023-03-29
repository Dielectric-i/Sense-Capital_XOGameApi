using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sense_Capital_XOGameApi.Interfaces;
using Sense_Capital_XOGameApi.Models;
using Sense_Capital_XOGameApi.RequestModels;
using Sense_Capital_XOGameApi.Services;

namespace Sense_Capital_XOGameApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        IValidator<RqstCreatePlayer> _rqstCreatePlayerValidator;

        public PlayerController(IPlayerService playerService, IValidator<RqstCreatePlayer> rqstCreatePlayerValidator)
        {
            _playerService = playerService;
            _rqstCreatePlayerValidator = rqstCreatePlayerValidator;

        }

        [HttpPut]
        public async Task<ActionResult<Player>> CreatePlayerAsync(RqstCreatePlayer rqstCreatePlayer)
        {
            try
            {
                var validationResult = await _rqstCreatePlayerValidator.ValidateAsync(rqstCreatePlayer);
                if (!validationResult.IsValid)
                    return Problem(400, "Validation error occurred in the CreatePlayerAsync method: " + validationResult.ToString(", "));
                
                return await _playerService.CreatePlayerAsync(rqstCreatePlayer);
            }
            catch (Exception ex)
            {
                return Problem(500, "An error occurred in the CreatePlayerAsync method: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayerByIdAsync(int id)
        {
            try
            {
                if (id<0)
                    return Problem(400, $"Validation error occurred in the GetPlayerByIdAsync method: {id} is not valid");

                return await _playerService.GetPlayerByIdAsync(id);

            }
            catch (Exception ex)
            {
                return Problem(500, "An error occurred in the GetPlayerByIdAsync method: " + ex.Message);
            }

        }

        [HttpGet("name/{id}")]
        public async Task<ActionResult<Player>> GetPlayerByNameAsync(int id)
        {
            try
            {
                if (id < 0)
                    return Problem(400, $"Validation error occurred in the GetPlayerByIdAsync method: {id} is not valid");

                return await _playerService.GetPlayerByIdAsync(id);

            }
            catch (Exception ex)
            {
                return Problem(500, "An error occurred in the GetPlayerByIdAsync method: " + ex.Message);
            }

        }

        //-----------------------------------------------------------------------
        private ObjectResult Problem(int status, string detail)
        {
            try
            {
                var problemDetails = new ProblemDetails
                {
                    Status = status,
                    Title = "PlayerController API:",
                    Detail = detail
                };

                return new ObjectResult(problemDetails);
            }
            catch (Exception ex)
            {

                return Problem(500, "An error occurred in the Problem method: " + ex.Message);
            }
        }
    }
}
