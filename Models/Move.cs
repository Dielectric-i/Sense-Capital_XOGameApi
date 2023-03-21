using System.Runtime.Serialization;

namespace Sense_Capital_XOGameApi.Models
{
    [DataContract(IsReference = true)]
    public class Move
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
