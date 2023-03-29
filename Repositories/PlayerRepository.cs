using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sense_Capital_XOGameApi.Data;
using Sense_Capital_XOGameApi.Interfaces;
using Sense_Capital_XOGameApi.Models;
using Sense_Capital_XOGameApi.Validation;
using Sense_Capital_XOGameApi.Validation.Rules;
using System.Numerics;
using FluentValidation.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Sense_Capital_XOGameApi.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ApiContext _context;
        private readonly IValidator<Player> _playerValidator;
        public PlayerRepository(ApiContext context, IValidator<Player> playerValidator)
        {
            _context = context;
            _playerValidator = playerValidator;
        }

        // Create new Player
        public async Task<Player> CreatePlayerAsync(Player player)
        {
            try
            {
                var validationResult = await _playerValidator.ValidateAsync(player);
                if (!validationResult.IsValid)
                    throw new Exception("The 'player' parameter is not valid: " + validationResult.ToString(", "));

                _context.Players.Add(player);
                await _context.SaveChangesAsync();
                return await GetPlayerByIdAsync(player.Id);
            }
            catch (Exception ex)
            {

                throw new Exception("PlayerRepository API: An error occurred in the is CreatePlayerAsync method: " + ex.Message);
            }
        }

        // Get Player by Id
        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            try
            {
                if (id < 0)
                    throw new ArgumentException($"Invalid player id: {id}");

                var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == id);
                return player;
            }
            catch (Exception ex)
            {
                throw new Exception("PlayerRepository API: An error occurred in the is GetPlayerByIdAsync method: " + ex.Message);
            }

        }

        // Get Player by Name
        public async Task<Player> GetPlayerByNameAsync(string name)
        {

            var player = await _context.Players.FirstOrDefaultAsync(u => u.Name == name);
            return player;
        }

        // Checking for the existence of an player by id
        public async Task<bool> isPlayerExistById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException($"Invalid player id: {id}");


                return await _context.Players.AnyAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("PlayerRepository API: Checking for the existence of an player by id: " + ex.Message);
            }
        }

        // Checking for the existence of an player by name
        public async Task<bool> isPlayerExistByName(string name)
        {
            try
            {
                var p = new Player() { Name = name };
                var validatonRezult = _playerValidator.Validate(p, op =>
                op.IncludeProperties(p => p.Name));
                if (!validatonRezult.IsValid)
                {
                    throw new ArgumentException("Parameter 'name' is not valid: " + validatonRezult.ToString(", "));
                }


                return await _context.Players.AnyAsync(p => p.Name == name);
            }
            catch (Exception ex)
            {
                throw new Exception("PlayerRepository API: An error occurred in the is PlayerExistByName method: " + ex.Message);
            }
        }

        // Delete Player By Id
        public async Task DeletePlayerByIdAsync(int id)
        {
            try
            {
                if (id < 0)
                    throw new ArgumentException($"Invalid player id: {id}");

                var player = await GetPlayerByIdAsync(id);
                _context.Remove(player);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("PlayerRepository API: An error occurred in the is DeletePlayerByIdAsync method: " + ex.Message);
            }
        }
    }
}