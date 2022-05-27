using BlazorPokerPlanning.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Shared.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByConnectionId(string connectionId);

        Task DeleteByConnectionId(string connectionId);
    }
}
