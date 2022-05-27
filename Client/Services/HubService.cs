using BlazorPokerPlanning.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorPokerPlanning.Shared.Models;
using RoomState = BlazorPokerPlanning.Client.Pages.RoomState;

namespace BlazorPokerPlanning.Client.Services
{
    public class HubService : IHubService
    {
        private HubConnection _hubConnection;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public HubService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InitializeAsync(string roomId, Action stateHasChanged, RoomState roomState)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var navigationManager = scope.ServiceProvider.GetRequiredService<NavigationManager>();

                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(navigationManager.ToAbsoluteUri($"/planninghub?roomId={roomId}"))
                    .Build();

                RegisterHandlers(stateHasChanged, roomState);

                await _hubConnection.StartAsync();

                await _hubConnection.InvokeAsync("GetVotes", roomId);
            }
        }

        public Task Send(string roomId, string voteValue)
        {
            return _hubConnection.SendAsync("Vote", voteValue, roomId);
        }

        public bool IsConnected(string roomId)
        {
            return _hubConnection.State == HubConnectionState.Connected;
        }

        public Task ChangeRoomEstimationPack(string roomId, int estimationPackId)
        {
            return _hubConnection.SendAsync("ChangeRoomEstimationPack", roomId, estimationPackId);
        }

        public string GetConnectionId() => _hubConnection.ConnectionId;

        public async ValueTask DisposeAsync(string roomId)
        {
            await _hubConnection.DisposeAsync();
        }

        public Task ResetVotes(string roomId)
        {
            return _hubConnection.SendAsync("ResetVotes", roomId);
        }
        
        public Task RevealVotes(string roomId)
        {            
            return _hubConnection.SendAsync("RevealVotes", roomId);
        }

        public Task ChangeIsAVoterStatus(string roomId)
        {
            return _hubConnection.SendAsync("ChangeIsAVoterStatus", roomId);
        }
        public Task ChangeVotedStatus(bool status)
        {
            return _hubConnection.SendAsync("ChangeVotedStatus", status);
        }

        private void RegisterHandlers(Action stateHasChanged, RoomState roomState)
        {
            _hubConnection.On<RoomStateDto>("RoomInitialized", (roomStateDto) =>
            {
                roomState.State = roomStateDto.RoomState;
                roomState.RoomCreatedBy = roomStateDto.RoomCreatedBy;
                roomState.User = roomStateDto.User;
                roomState.EstimationPacks = roomStateDto.EstimationPacks;
                roomState.SelectedEstimationPack = roomStateDto.SelectedEstimationPack;
                stateHasChanged();
            });

            _hubConnection.On<VotesQueueChangedDto>($"VotesQueueChanged", (votesQueueChangedDto) =>
            {
                roomState.State = votesQueueChangedDto.RoomState;
                roomState.Votes.RemoveAll(_ => true);
                roomState.Votes.AddRange(votesQueueChangedDto.Votes);

                stateHasChanged();
            });

            _hubConnection.On<BlazorPokerPlanning.Shared.Models.RoomState>("RoomStateChanged", (state) =>
            {
                roomState.State = state;

                stateHasChanged();
            });

            _hubConnection.On<int>("RoomEstimationPackChanged", (estimationPackId) =>
            {
                roomState.SelectedEstimationPack = roomState.EstimationPacks.SingleOrDefault(x => x.Id == estimationPackId);

                stateHasChanged();
            });

            _hubConnection.On<User>("UserStatusChanged", (user) =>
            {
                roomState.User = user;

                stateHasChanged();
            });
            _hubConnection.On<User>("UserVotedChanged", (user) =>
            {
                roomState.User = user;
                stateHasChanged();
            });
            
        }


    }
}
