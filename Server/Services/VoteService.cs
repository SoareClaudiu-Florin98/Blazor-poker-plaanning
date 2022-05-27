using BlazorPokerPlanning.Shared.Entities;
using BlazorPokerPlanning.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Server.Services
{
    public class VoteService : IVoteService
    {
        private readonly IUserRepository userRepository;
        private readonly IVoteRepository voteRepository;

        public VoteService(IUserRepository userRepository, IVoteRepository voteRepository)
        {
            this.userRepository = userRepository;
            this.voteRepository = voteRepository;
        }

        public async Task<Vote> CreateVote(string voteValue, string connectionId, Guid roomId)
        {
            var users = await userRepository.GetAllAsync();
            var connectedUserToRoom = users.FirstOrDefault(x => x.ConnectionId == connectionId && x.RoomId == roomId);

            if (connectedUserToRoom != null)
            {
                var vote = new Vote();

                vote.Id = Guid.NewGuid();
                vote.RoomId = roomId;
                vote.UserId = connectedUserToRoom.Id;
                vote.Value = voteValue;

                await voteRepository.InsertAsync(vote);

                return vote;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Vote>> GetVotesForRoom(Guid roomId)
        {
            return await voteRepository.GetAllByRoomId(roomId);
        }

        public async Task<bool> UpdateVote(string voteValue, Guid userId, Guid roomId)
        {
            var votes = await voteRepository.GetAllAsync();
            var voteToUpdate = votes.FirstOrDefault(x => x.RoomId == roomId && x.UserId == userId);

            if (voteToUpdate == null) return false;

            voteToUpdate.Value = voteValue;
            await voteRepository.UpdateAsync(voteToUpdate);

            return true;
        }

        public async Task ResetRoomVotes(Guid roomId)
        {
            var votes = await voteRepository.GetAllByRoomId(roomId);

            foreach (var vote in votes)
            {
                vote.Value = null;
            }

            await voteRepository.UpdateBatchAsync(votes);
        }

        public async Task DeleteForUser(Guid roomId, Guid userId)
        {
            var votes = await voteRepository.GetAllByRoomId(roomId);
            var userVotes = votes.Where(v => v.UserId == userId).ToList();

            foreach (var userVote in userVotes)
            {
                await voteRepository.DeleteAsync(userVote);
            }
        }
    }
}
