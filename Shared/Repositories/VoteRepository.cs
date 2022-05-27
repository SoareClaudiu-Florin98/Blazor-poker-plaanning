using BlazorPokerPlanning.Shared.Context;
using BlazorPokerPlanning.Shared.Entities;
using BlazorPokerPlanning.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Shared.Repositories
{
    public class VoteRepository : GenericRepository<Vote>, IVoteRepository
    {
        public VoteRepository(PokerPlanningDbContext pokerPlanningDbContext) : base(pokerPlanningDbContext)
        {
        }

        public async Task<List<Vote>> GetAllByRoomId(Guid roomId)
        {
            return await _database.Set<Vote>().Where(v => v.RoomId == roomId).ToListAsync();
        }

        public async Task UpdateBatchAsync(List<Vote> votes)
        {
            _database.Set<Vote>().UpdateRange(votes);
            await _database.SaveChangesAsync();
        }
    }
}
