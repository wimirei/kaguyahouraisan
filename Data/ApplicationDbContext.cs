using Microsoft.EntityFrameworkCore;
using thrucommunity.Models;

namespace thrucommunity.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ReplayModel> Replays { get; set; }

        public DbSet<PlayerModel> Players { get; set; }
    }
}
