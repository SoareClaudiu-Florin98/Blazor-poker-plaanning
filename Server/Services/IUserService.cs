using BlazorPokerPlanning.Shared.Entities;
using System;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Server.Services
{
    public interface IUserService
    {
        Task<User> CreateUser(Guid roomId, string connectionId);

        Task<User> GetUserByConnectionId(string connectionId);

        Task<(bool Updated, User UpdatedUser)> SwitchIsAVoterStatus(string connectionId);
        Task<(bool Updated, User UpdatedUser)> SwitchVotedStatus(string connectionId, bool status);

        Task DeleteUser(string connectionId);
    }
}
