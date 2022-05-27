using BlazorPokerPlanning.Shared.Entities;
using BlazorPokerPlanning.Shared.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<User> CreateUser(Guid roomId, string connectionId)
        {
            var newUser = new User();

            newUser.ConnectionId = connectionId;
            newUser.CreationDate = DateTime.Now;
            newUser.RoomId = roomId;
            newUser.IsAVoter = true;

            await userRepository.InsertAsync(newUser);

            return newUser;
        }

        public async Task DeleteUser(string connectionId)
        {
            await userRepository.DeleteByConnectionId(connectionId);
        }

        public async Task<User> GetUserByConnectionId(string connectionId) => await userRepository.GetByConnectionId(connectionId);

        public async Task<(bool Updated, User UpdatedUser)> SwitchIsAVoterStatus(string connectionId)
        {
            var user = await userRepository.GetByConnectionId(connectionId);

            if (user == null)
            {
                return (false, null);
            }

            user.IsAVoter = !user.IsAVoter;

            return (true, user);
        }
        public async Task<(bool Updated, User UpdatedUser)> SwitchVotedStatus(string connectionId, bool status)
        {
            var user = await userRepository.GetByConnectionId(connectionId);
            if (user == null)
            {
                return (false, null);
            }
            user.Voted = status;

            return (true, user);
        }
    }
}
