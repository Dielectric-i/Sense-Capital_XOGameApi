namespace Sense_Capital_XOGameApi.Models
{
    public class Move
    {
        public Move()
        {
            Player= new Player();
            Game= new Game();
        }
        public int Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
