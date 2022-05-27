using BlazorPokerPlanning.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Server.Services
{
    public interface IVoteService
    {
        Task<Vote> CreateVote(string voteValue, string connectionId, Guid roomId);

        Task<List<Vote>> GetVotesForRoom(Guid roomId);

        Task<bool> UpdateVote(string voteValue, Guid userId, Guid roomId);

        Task DeleteForUser(Guid roomId, Guid userId);

        Task ResetRoomVotes(Guid guid);
    }
}
