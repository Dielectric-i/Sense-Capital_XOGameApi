using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sense_Capital_XOGameApi.Interfaces;
using Sense_Capital_XOGameApi.Models;
using Sense_Capital_XOGameApi.RequestModels;

namespace Sense_Capital_XOGameApi.Services
{
    public class PlayerService : IPlayerService
    {
        public readonly IPlayerRepository _playerRepository;
        public readonly IValidator<RqstCreatePlayer> _rqstCreatePlayerValidator;

        public PlayerService(
            IPlayerRepository playerRepository,
            IValidator<RqstCreatePlayer> rqstCreatePlayerValidator)
        {
            _playerRepository = playerRepository;
            _rqstCreatePlayerValidator = rqstCreatePlayerValidator;
        }

        // Create new player
        public async Task<ActionResult<Player>> CreatePlayerAsync(RqstCreatePlayer rqstCreatePlayer)


        {
            try
            {
                var validationResult = await _rqstCreatePlayerValidator.ValidateAsync(rqstCreatePlayer);
                if (!validationResult.IsValid)
                    return Problem(400, "Validation error occurred in the CreatePlayerAsync method: " + validationResult.ToString(", "));


                // Проверяем существует ли игрок с таким именем
                bool isPlayerExist = await _playerRepository.isPlayerExistByName(rqstCreatePlayer.Name);

                // Если игрок с таким именем - возвращаем 400
                if (isPlayerExist)
                    return Problem(400, "Player already exiists.");

                // Создаем игрока
                var newPlayer = new Player() { Name = rqstCreatePlayer .Name};

                var player = await _playerRepository.CreatePlayerAsync(newPlayer);

                return new CreatedResult($"api/Player/{player.Id}", player);
            }
            catch (Exception ex)
            {
                return Problem(500, "An error occurred in the CreatePlayerAsync method: " + ex.Message);
            }
        }

        // Get Player by Id
        public async Task<ActionResult<Player>> GetPlayerByIdAsync(int id)
        {
            try
            {
                if (id < 0)
                    return Problem(400, $"Validation error occurred in the DeletePlayerByIdAsync method: {id} is not valid");

                var player = await _playerRepository.GetPlayerByIdAsync(id);
                return new OkObjectResult(player);
            }
            catch (Exception ex)
            {

                return Problem(500, "An error occurred in the is GetPlayerByIdAsync method: " + ex.Message);
            }

        }

        // Delete Player By Id
        public async Task<ActionResult> DeletePlayerByIdAsync(int id)
        {
            try
            {
                if (id < 0)
                    return Problem(400, $"Validation error occurred in the DeletePlayerByIdAsync method: {id} is not valid");

                await _playerRepository.DeletePlayerByIdAsync(id);

                return new NoContentResult();
            }
            catch (Exception ex)
            {

                return Problem(500, "An error occurred in the DeletePlayerById method: " + ex.Message);
            }
            
        }

        
        private ObjectResult Problem(int status, string detail)
        {
            try
            {
                var problemDetails = new ProblemDetails
                {
                    Status = status,
                    Title = "PlayerService API:",
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
