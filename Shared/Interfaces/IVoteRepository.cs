using BlazorPokerPlanning.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Shared.Interfaces
{
    public interface IVoteRepository : IRepository<Vote>
    {
        Task<List<Vote>> GetAllByRoomId(Guid roomId);

        Task UpdateBatchAsync(List<Vote> votes);
    }
}
