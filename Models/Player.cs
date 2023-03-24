using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sense_Capital_XOGameApi.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 50 символов")]
        public string Name { get; set; }

        [JsonIgnore]
        public List<Game> Games { get; set; } = new List<Game>();
    }
}
