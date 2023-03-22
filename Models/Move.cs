using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Sense_Capital_XOGameApi.Models
{

    public class Move
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int GameId { get; set; }

        [JsonIgnore]
        public Game Game { get; set; }
    }
}
