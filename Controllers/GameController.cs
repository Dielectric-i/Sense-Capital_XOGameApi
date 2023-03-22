using Microsoft.AspNetCore.Mvc;
using Sense_Capital_XOGameApi.Models;
using Sense_Capital_XOGameApi.RequestModels;
using System.ComponentModel.DataAnnotations;

namespace Sense_Capital_XOGameApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
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
        ///        "player1Name": "Игрок 1",
        ///        "player2Name": "Игрок 2"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created a Game</response>
        /// <response code="400">If the arguments are not correct</response>
        /// <response code="500">if there was an internal server error.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<Game>> CreateGameAsync([Required][FromBody] RqstCreateGame rqstCreateGame)
        {
            try
            {
                var game = await _gameService.CreateGameAsync(rqstCreateGame.Player1Name, rqstCreateGame.Player2Name);
                return game;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGameAsync(int id)
        {
            try
            {
                return await _gameService.GetGameAsync(id); ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Delete an Game by Id
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
        public async Task<IActionResult> DeleteGameAsync(int id)
        {
            await _gameService.DeleteGameAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Make a move in a game
        /// </summary>
        /// <returns>Current game whith the move</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: /api/Game/{id}/move
        ///     {
        ///     "row": 0,
        ///     "column": 0,
        ///     "playerId": 0,
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("move")]
        public async Task<ActionResult<Game>> MakeMove(RqstMakeMove rqstMakeMove)
        {
            try
            {
                return await _gameService.MakeMove(rqstMakeMove);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}