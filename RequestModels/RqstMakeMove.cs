using Sense_Capital_XOGameApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Sense_Capital_XOGameApi.RequestModels
{
    public class RqstMakeMove
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public int PlayerId { get; set; }

        public int GameId { get; set; }
    }
}
