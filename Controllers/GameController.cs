using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sense_Capital_XOGameApi.Models;
using Sense_Capital_XOGameApi.RequestModels;

namespace Sense_Capital_XOGameApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IValidator<RqstCreateGame> _rqstCreateGameValidator;
        private readonly IValidator<RqstMakeMove> _rqstMakeMoveValidator;

        public GameController(
            IGameService gameService,
            IValidator<RqstCreateGame> rqstCreateGameValidator,
            IValidator<RqstMakeMove> rqstMakeMoveValidator
            )
        {
            _gameService = gameService;
            _rqstCreateGameValidator = rqstCreateGameValidator;
            _rqstMakeMoveValidator = rqstMakeMoveValidator;
        }


        /// <summary>
        /// Create new game
        /// </summary>
        /// <returns>A newly created a Game</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: api/Game
        ///     {
        ///        "P1Name": "Игрок 1",
        ///        "P2Name": "Игрок 2"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created a game object</response>
        /// <response code="400">If the request are not correct</response>
        /// <response code="500">If there was an internal server error.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<ActionResult<Game>> CreateGameAsync(RqstCreateGame? rqstCreateGame)
        {
                var validationResult = await _rqstCreateGameValidator.ValidateAsync(rqstCreateGame);
               // if (!validationResult.IsValid)
                 //   return Problem( statusCode: 422, detail: validationResult.ToString(", "));
                
                return await _gameService.CreateGameAsync(rqstCreateGame);
        }

        /// <summary>
        /// Get all Games
        /// </summary>
        /// <returns>All Games</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: api/Game
        ///     
        /// </remarks>
        /// <response code="200">If the request was successful and at least one Game is returned.</response>
        /// <response code="204">If the request was successful but the Game list is empty (contains no items).</response>
        /// <response code="400">If there was an error while processing the request. In this case, the error message will be included in the response body.</response>
        /// <response code="500">if there was an internal server error.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetAllGamesAsync()
        {
            try
            {
                var result = await _gameService.GetAllGamesAsync();
                if (result is OkObjectResult)
                {
                    var games = result.Result as IEnumerable<Game>;
                    return Ok(games);
                }
                return result;
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        /// <summary>
        /// Delete all Games
        /// </summary>
        /// <returns>Empty body, 204 response</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete: api/Game
        ///     
        /// </remarks>
        /// <response code="204">If the request was successful.</response>
        /// <response code="500">if there was an internal server error.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [HttpDelete]
        public async Task<ActionResult> DeleteAllGamesAsync()
        {
            try
            {
                return await _gameService.DeleteAllGamesAsync();
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        /// <summary>
        /// Get a game by id
        /// </summary>
        /// <returns> A game by its ID</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: api/Game/{id}
        ///
        /// </remarks>
        /// <response code="200">If the request was successful and at least one Game is returned.</response>
        /// <response code="400">If there was an error while processing the request. In this case, the error message will be included in the response body.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGameByIdAsync(int id)
        {
            try
            {
                if (id < 0)
                    return Problem(statusCode: 422, detail: $"id: {id} is not valid");

                return await _gameService.GetGameAsync(id);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        /// <summary>
        /// Delete a Game by Id
        /// </summary>
        /// <returns>All Games</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: api/Game/Delete/{id}
        ///     
        /// </remarks>
        /// <response code="204">If the request was successful but the Game list is empty (contains no items).</response>
        /// <response code="400">If there was an error while processing the request. In this case, the error message will be included in the response body.</response>
        /// <response code="500">if there was an internal server error.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameByIdAsync(int id)
        {
            try
            {
                if (id < 0)
                    return Problem(statusCode: 422, detail: $"id: {id} is not valid");

                return await _gameService.DeleteGameAsync(id);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        /// <summary>
        /// Make a move in a game
        /// </summary>
        /// <returns>Current game whith the move</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: /api/Game/move
        ///     {
        ///     "row": 0,
        ///     "column": 0,
        ///     "playerId": 0,
        ///     "gameId": 0
        ///     }
        ///
        /// </remarks>
        /// 
        /// 
        /// 
        /// <response code="200">If the request was successful and at least one Game is returned.</response>
        /// <response code="204">If the request was successful but the Game list is empty (contains no items).</response>
        /// <response code="400">If there was an error while processing the request. In this case, the error message will be included in the response body.</response>
        /// <response code="500">if there was an internal server error.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPut("move")]
        public async Task<ActionResult<Game>> MakeMove(RqstMakeMove rqstMakeMove)
        {
            try
            {
                var validationResult = await _rqstMakeMoveValidator.ValidateAsync(rqstMakeMove);
                if (!validationResult.IsValid)
                    return Problem(statusCode: 422, detail: validationResult.ToString(", "));

                return await _gameService.MakeMoveAsync(rqstMakeMove);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }
    }
}