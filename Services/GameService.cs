﻿using FluentValidation;
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
        public readonly IMoveRepository _moveRepository;
        public readonly IValidator<RqstCreateGame> _rqstCreateGameValidator;
        public readonly IValidator<RqstMakeMove> _rqstMakeMoveValidator;

        public GameService(
            IGameRepository gameRepository,
            IPlayerRepository playerRepository,
            IMoveRepository moveRepository,
            IValidator<RqstCreateGame> qstCreateGameValidator,
            IValidator<RqstMakeMove> rqstMakeMoveValidator
            )
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _moveRepository = moveRepository;

            _rqstCreateGameValidator = qstCreateGameValidator;
            _rqstMakeMoveValidator = rqstMakeMoveValidator;
        }

        public async Task<ActionResult<Game>> CreateGameAsync(RqstCreateGame rqstCreateGame)
        {
            var validationResult = await _rqstCreateGameValidator.ValidateAsync(rqstCreateGame);
            if (!validationResult.IsValid)
                return Problem(400, "Validation error occurred in the CreateGameAsync method: " + validationResult.ToString(", "));

            try
            {
                // Пробуем искать переданное имя в бд
                var player1 = await _playerRepository.GetByName(rqstCreateGame.P1Name);

                // Если не находится, тогда берем имя, которое было передано в запросе и создаем нового игрока
                if (player1 == null)
                {
                    player1 = new Player { Name = rqstCreateGame.P1Name };
                    await _playerRepository.CreatePlayerAsync(player1);
                }

                var player2 = await _playerRepository.GetByName(rqstCreateGame.P2Name);
                if (player2 == null)
                {
                    player2 = new Player();
                    player2.Name = rqstCreateGame.P2Name;
                    await _playerRepository.CreatePlayerAsync(player2);
                }

                // Создаем игру и сохраняем в бд
                Game newGame = new Game()
                {
                    Players = new List<Player>() { player1, player2 },
                    CurrentPlayerId = player1.Id
                };

                await _gameRepository.CreateGameAsync(newGame);

                return new CreatedResult($"api/Game/{newGame.Id}", newGame);

            }
            catch (Exception ex)
            {
                return Problem(500, "An error occurred in the CreateGameAsync method: " + ex.Message);
            }
        }

        public async Task<ActionResult<Game>> GetGameAsync(int id)
        {
            if (id < 0)
                return Problem(400, $"Validation error occurred in the GetGameAsync method: {id} is not valied");

            try
            {
                var game = await _gameRepository.GetGameByIdAsync(id);
                if (game == null)
                {
                    return Problem(400, "An error occurred in the GetGameAsync method: " + $"Game with id {id} not found.");
                }
                return game;
            }
            catch (Exception ex)
            {
                return Problem(500, "An error occurred in the GetGameAsync method: " + ex.Message);
            }
        }

        public async Task<ActionResult<IEnumerable<Game>>> GetAllGamesAsync()
        {
            try
            {
                var games = await _gameRepository.GetAllGamesAsync();
                if (games.Count() == 0)
                    return new NoContentResult();

                return new OkObjectResult(games);
            }
            catch (Exception ex)
            {
                return Problem(500, "An error occurred in the GetAllGamesAsync method: " + ex.Message);
            }
        }

        public async Task<ActionResult> DeleteGameAsync(int id)
        {
            if (id < 0)
                return Problem(400, $"Validation error occurred in the DeleteGameAsync method: {id} is not valied");

            try
            {
                await _gameRepository.DeleteAsync(id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return Problem(500, "An error occurred in the DeleteGameAsync method: " + ex.Message);
            }
        }

        public async Task<ActionResult<Game>> MakeMoveAsync(RqstMakeMove rqstMakeMove)
        {
            var validationResult = await _rqstMakeMoveValidator.ValidateAsync(rqstMakeMove);
            if (!validationResult.IsValid)
                return Problem(400, "Validation error occurred in the MakeMove method: " + validationResult.ToString(", "));

            try
            {
                // Поиск игры по id
                var game = await _gameRepository.GetGameByIdAsync(rqstMakeMove.GameId);

                // Проверка наличия игры в бд
                if (game == null)
                    return Problem(400, $"The game does not exist. Requested id: {rqstMakeMove.GameId}");

                // Проверка закончена игра или нет
                if (game.WinnerId != null)
                    return Problem(400, $"The game is already finished. Winner Id: {game.WinnerId}");

                // Проверка правильный ли игрок ходит
                if (game.CurrentPlayerId != rqstMakeMove.PlayerId)
                {
                    // Проверка существует ли игрок
                    bool isPlayerExist = await _playerRepository.isPlayerExist(rqstMakeMove.PlayerId);
                    if (!isPlayerExist)
                        return Problem(400, $"The player does not exist. Requested Player Id: {rqstMakeMove.PlayerId}");

                    return Problem(400, $"It's not your turn. Current Player Id: {game.CurrentPlayerId}");
                }

                // Проверка ход в пределах поля
                if (rqstMakeMove.Row < 0 || rqstMakeMove.Row > 2 || rqstMakeMove.Column < 0 || rqstMakeMove.Column > 2)
                    return Problem(400, "Invalid move. Move out of bounds");

                // Проверка ячейка сводобна
                if (game.BoardState[rqstMakeMove.Row * 3 + rqstMakeMove.Column] != '-')
                    return Problem(400, "This cell is already occupied.");

                // Заполнение ячейки
                var playerSymbol = rqstMakeMove.PlayerId == game.Players[0].Id ? game.Player1Symbol : game.Player2Symbol;
                game.BoardState = game.BoardState.Substring(0, rqstMakeMove.Row * 3 + rqstMakeMove.Column) +
                                  playerSymbol +
                                  game.BoardState.Substring(rqstMakeMove.Row * 3 + rqstMakeMove.Column + 1);

                // Проверка выигрыша
                var result = CheckResult(game.BoardState);
                if (result != null)
                {
                    game.WinnerId = result == "Draw" ? null : rqstMakeMove.PlayerId;
                }

                // Переключение игрока
                else
                {
                    game.CurrentPlayerId = game.Players[0].Id == rqstMakeMove.PlayerId ? game.Players[1].Id : game.Players[0].Id;
                }

                // Создаем move в бд
                var move = new Move
                {
                    Row = rqstMakeMove.Row,
                    Column = rqstMakeMove.Column,
                    PlayerId = rqstMakeMove.PlayerId,
                    Game = game,
                };
                await _moveRepository.CreateMoveAsync(move);

                return new OkObjectResult(game);
            }
            catch (Exception ex)
            {
                return Problem(500, "An error occurred in the MakeMoveAsync method: " + ex.Message);
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

        private ObjectResult Problem(int status, string detail)
        {
            var problemDetails = new ProblemDetails
            {
                Status = status,
                Title = "GameService API:",
                Detail = detail
            };

            return new ObjectResult(problemDetails);
        }
    }
}
