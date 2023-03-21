using Microsoft.AspNetCore.Mvc;
using Sense_Capital_XOGameApi.Controllers;
using Sense_Capital_XOGameApi.Interfaces;
using Sense_Capital_XOGameApi.Models;
using Sense_Capital_XOGameApi.RequestModels;

namespace Sense_Capital_XOGameApi.Services
{
    public class GameService : IGameService
    {
        public readonly IGameRepository _gameRepository;
        public readonly IPlayerRepository _playerRepository;

        public GameService(IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }

        public async Task<ActionResult<Game>> CreateGameAsync(string player1Name, string player2Name)
        {
            try
            {
                //Пробуем искать переданное имя в бд
                var newPlayer1 = await _playerRepository.GetByName(player1Name);


                //Если не находится, тогда берем имя, которое было переданно в запросе и создаем нового игрока
                if (newPlayer1 == null)
                {
                    newPlayer1 = new Player { Name = player1Name };
                    await _playerRepository.CreatePlayerAsync(newPlayer1);
                }


                var newPlayer2 = await _playerRepository.GetByName(player2Name);
                if (newPlayer2 == null)
                {
                    newPlayer2 = new Player();
                    newPlayer2.Name = player2Name;
                    await _playerRepository.CreatePlayerAsync(newPlayer2);
                }
                // Создаем игру и сохраняем в бд
                Game newGame = new Game { Player1 = newPlayer1, Player2 = newPlayer2, CurrentPlayerId = newPlayer1.Id };
                await _gameRepository.CreateGameAsync(newGame);

                //return newGame;
                return new CreatedAtActionResult("GetGame", "Game", new { id = newGame.Id }, newGame);
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while creating the Game",
                    Detail = ex.Message
                };

                return new ObjectResult(problemDetails);
            }
        }

        public async Task<ActionResult<Game>> GetGameAsync(int id)
        {
            try
            {
                var game = await _gameRepository.GetGameAsync(id);
                await PutPlayersToGame(game);
                return game;
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while getting a Game by id",
                    Detail = ex.Message
                };

                return new ObjectResult(problemDetails);
            }
        }

        public async Task<ActionResult<IEnumerable<Game>>> GetAllGamesAsync()
        {
            try
            {
                var games = await _gameRepository.GetAllGamesAsync();
                if (games.Count() == 0)
                    return new NoContentResult();
                foreach (var game in games)
                {
                    await PutPlayersToGame(game);
                }
                return new OkObjectResult(games);
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while getting the list of games:",
                    Detail = ex.Message
                };

                return new ObjectResult(problemDetails);
            }
        }

        public async Task<ActionResult> DeleteGameAsync(int id)
        {
            try
            {
                await _gameRepository.DeleteAsync(id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while trying to delete a Game",
                    Detail = ex.Message
                };

                return new ObjectResult(problemDetails);
            }
        }

        // Помещаем игроков из бд в игру
        public async Task<ActionResult<Game>> PutPlayersToGame(Game game)
        {
            try
            {
                game.Player1 = await _playerRepository.GetById(game.Player1Id);
                game.Player2 = await _playerRepository.GetById(game.Player2Id);
                return game;
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while trying to delete an Game",
                    Detail = ex.Message
                };

                return new ObjectResult(problemDetails);
            }
        }

        public async Task<ActionResult<Game>> MakeMove(RqstMakeMove rqstMakeMove)
        {
            try
            {
                // Поиск игры по id
                var game = await _gameRepository.GetGameAsync(rqstMakeMove.GameId);
                await PutPlayersToGame(game);

                // Проверка закончена игра или нет
                if (game.WinnerName != null)
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = 400,
                        Title = "The game is already finished.",
                        Detail = "Winner Name: " + game.WinnerName
                    };

                    return new ObjectResult(problemDetails);
                }

                // Проверка правильный ли грок ходит
                if (game.CurrentPlayerId != rqstMakeMove.PlayerId)
                {
                    Problem(
                        400, 
                        "It's not your turn.", 
                        "Current Player Id: " + game.CurrentPlayerId);
                }

                // Проверка возможности хода
                if (rqstMakeMove.Row < 0 || rqstMakeMove.Row > 2 || rqstMakeMove.Column < 0 || rqstMakeMove.Column > 2)
                {
                    Problem(
                        400,
                        "Invalid move.",
                        "Move out of bounds");
                }
                if (game.BoardState[rqstMakeMove.Row * 3 + rqstMakeMove.Column] != '-')
                {
                    Problem(
                        400,
                        "This cell is already occupied.",
                        "Try another cell");
                }
                
                // Здесь положить move в бд
               // var move = await 

                // Заполнение ячейки
                var playerSymbol = rqstMakeMove.PlayerId == game.Player1.Id ? game.Player1Symbol : game.Player2Symbol;
                game.BoardState = game.BoardState.Substring(0, rqstMakeMove.Row * 3 + rqstMakeMove.Column) +
                                  playerSymbol +
                                  game.BoardState.Substring(rqstMakeMove.Row * 3 + rqstMakeMove.Column + 1);

                // Проверка выигрыша
                var result = CheckResult(game.BoardState);
                if (result != null)
                {
                    game.WinnerName = result == "Draw" ? null : move.Player.Name;
                }
                else
                {
                    // Переключение игрока
                    game.CurrentPlayerId = game.Player1.Id == move.Player.Id ? game.Player1.Id : game.Player2.Id;
                }

                // Добавляем ход в игру
                game.Moves.Add(move);

                // Обновление игры в бд
                await _gameRepository.UpdateAsync(game.Id);
                return game;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string CheckResult(string boardState)
        {
            var board = new string[3, 3];
            var index = 0;
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    board[i, j] = boardState[index].ToString() == "-" ? null : boardState[index].ToString();
                    index++;
                }
            }

            // Check rows
            for (var i = 0; i < 3; i++)
            {
                if (board[i, 0] != null && board[i, 0] == board[i, 1] && board[i, 0] == board[i, 2])
                {
                    return board[i, 0];
                }
            }

            // Check columns
            for (var j = 0; j < 3; j++)
            {
                if (board[0, j] != null && board[0, j] == board[1, j] && board[0, j] == board[2, j])
                {
                    return board[0, j];
                }
            }

            // Check diagonals
            if (board[0, 0] != null && board[0, 0] == board[1, 1] && board[0, 0] == board[2, 2])
            {
                return board[0, 0];
            }
            if (board[0, 2] != null && board[0, 2] == board[1, 1] && board[0, 2] == board[2, 0])
            {
                return board[0, 2];
            }

            // Check for a Draw
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (board[i, j] == null)
                    {
                        // There is an empty cell, so the game is not over yet
                        return null;
                    }
                }
            }

            // It's a Draw
            return "Draw";
        }

        private ObjectResult Problem(int status, string title, string detail)
        {
            var problemDetails = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail
            };

            return new ObjectResult(problemDetails);
        }
    }
}
