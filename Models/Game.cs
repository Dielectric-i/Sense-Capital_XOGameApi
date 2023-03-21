using System.Numerics;

namespace Sense_Capital_XOGameApi.Models
{
    public class Game
    {
        public Game()
        {
            Moves= new List<Move>();
            Player1= new Player();
            Player2= new Player();
        }
        public int Id { get; set; }
        public string Player1Symbol { get; set; } = "X";
        public string Player2Symbol { get; set; } = "O";
        public int CurrentPlayerId { get; set; }
        public string? WinnerName { get; set; }
        public string BoardState { get; set; } = "---------";

        public int Player1Id { get; set; }
        public Player Player1 { get; set; }

        public int Player2Id { get; set; }
        public Player Player2 { get; set; }

        public List<Move>? Moves { get; set; }
    }
}
