using BlazorPokerPlanning.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorPokerPlanning.Shared.Context
{
    public class PokerPlanningDbContext : DbContext
    {

        public PokerPlanningDbContext(DbContextOptions<PokerPlanningDbContext> options) : base(options)
        {
        }

        public DbSet<Room> Room { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Vote> Vote { get; set; }
    }
}
