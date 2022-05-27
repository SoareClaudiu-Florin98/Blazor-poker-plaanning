using System;
using System.Threading.Tasks;
using RoomState = BlazorPokerPlanning.Client.Pages.RoomState;

namespace BlazorPokerPlanning.Client.Services
{
    public interface IHubService
    {
        Task InitializeAsync(string roomId, Action stateHasChanged, RoomState roomState);

        Task Send(string roomId, string voteValue);

        Task ResetVotes(string roomId);

        Task RevealVotes(string roomId);

        Task ChangeIsAVoterStatus(string roomId);
        Task ChangeVotedStatus(bool status);

        bool IsConnected(string roomId);

        ValueTask DisposeAsync(string roomId);

        Task ChangeRoomEstimationPack(string roomId, int estimationPackId);

        string GetConnectionId();
    }
}
