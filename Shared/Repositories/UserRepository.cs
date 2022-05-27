using BlazorPokerPlanning.Shared.Context;
using BlazorPokerPlanning.Shared.Entities;
using BlazorPokerPlanning.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Shared.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(PokerPlanningDbContext pokerPlanningDbContext) : base(pokerPlanningDbContext)
        {
        }

        public async Task DeleteByConnectionId(string connectionId)
        {
            var user = await GetByConnectionId(connectionId);

            if (user == null)
            {
                return;
            }

            await DeleteAsync(user);
        }

        public async Task<User> GetByConnectionId(string connectionId)
        {
            return await _database.Set<User>().FirstOrDefaultAsync(x => x.ConnectionId == connectionId);
        }
    }
}
