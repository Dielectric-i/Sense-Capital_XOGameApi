using Microsoft.EntityFrameworkCore;
using Sense_Capital_XOGameApi.Models;

namespace Sense_Capital_XOGameApi.Data
{
    public class ApiContext:DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Move> Moves { get; set; }
    }
}
