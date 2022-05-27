using BlazorPokerPlanning.Shared.Context;
using BlazorPokerPlanning.Shared.Entities;
using BlazorPokerPlanning.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Shared.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(PokerPlanningDbContext pokerPlanningDbContext) : base(pokerPlanningDbContext)
        {
        }

        public override Task<Room> GetByIdAsync(Guid id)
        {
            return _database.Set<Room>().Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
