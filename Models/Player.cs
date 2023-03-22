using System.Text.Json.Serialization;

namespace Sense_Capital_XOGameApi.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //[JsonIgnore]
       // public List<Game> Games { get; set; } = new List<Game>();
    }
}
