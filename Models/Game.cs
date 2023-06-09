﻿
namespace Sense_Capital_XOGameApi.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Player1Symbol { get; set; } = "X";
        public string Player2Symbol { get; set; } = "O";
        public int CurrentPlayerId { get; set; }
        public int? WinnerId { get; set; }
        public string BoardState { get; set; } = "---------";

        public List<Player> Players { get; set; } = new List<Player>();

        public List<Move> Moves { get; set; } = new List<Move>();
    }
}
