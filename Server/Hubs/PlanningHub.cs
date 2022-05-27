using BlazorPokerPlanning.Server.Services;
using BlazorPokerPlanning.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Server.Hubs
{
    public class PlanningHub : Hub
    {
        private readonly IRoomService roomService;
        private readonly IVoteService voteService;
        private readonly IUserService userService;
        private readonly IEstimationPackService estimationPackService;

        public PlanningHub(IRoomService roomService, IVoteService voteService, IUserService userService, IEstimationPackService estimationPackService)
        {
            this.roomService = roomService;
            this.voteService = voteService;
            this.userService = userService;
            this.estimationPackService = estimationPackService;
        }

        public override async Task OnConnectedAsync()
        {
            var roomId = Guid.Parse(Context.GetHttpContext().Request.Query["roomId"]);

            var existingRoom = await roomService.GetRoomById(roomId);

            if (existingRoom == null)
            {
                existingRoom = await roomService.CreateRoom(Context.ConnectionId, roomId);
            }
            else
            {
                var user = await userService.CreateUser(roomId, Context.ConnectionId);

                await roomService.AttachUserToRoom(existingRoom, user);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            await voteService.CreateVote(null, Context.ConnectionId, existingRoom.Id);

            var allVotes = await voteService.GetVotesForRoom(existingRoom.Id);
            var currentUser = await userService.GetUserByConnectionId(Context.ConnectionId);
            var estimationPacks = estimationPackService.GetAll().ToList();

            await Clients.Group(existingRoom.Id.ToString()).SendAsync("VotesQueueChanged", new VotesQueueChangedDto
            {
                Votes = allVotes,
                RoomState = existingRoom.State
            });
            await Clients.Client(Context.ConnectionId).SendAsync("RoomInitialized", new RoomStateDto
            {
                RoomState = existingRoom.State,
                RoomCreatedBy = existingRoom.CreatedByUserId,
                User = currentUser,
                SelectedEstimationPack = estimationPacks.Single(x => x.Id == existingRoom.EstimationPackId),
                EstimationPacks = estimationPacks
            });

            await base.OnConnectedAsync();
        }

        public async Task Vote(string voteValue, string roomId)
        {
            var user = await userService.GetUserByConnectionId(Context.ConnectionId);

            var isUpdated = await voteService.UpdateVote(voteValue, user.Id, Guid.Parse(roomId));
            if (isUpdated)
            {
                await GetVotes(roomId);
            }
        }

        public async Task GetVotes(string roomId)
        {
            var room = await roomService.GetRoomById(Guid.Parse(roomId));
            var votes = await voteService.GetVotesForRoom(Guid.Parse(roomId));

            await Clients.Group(roomId).SendAsync("VotesQueueChanged", new VotesQueueChangedDto
            {
                Votes = votes,
                RoomState = room.State
            });
        }

        public async Task ResetVotes(string roomId)
        {
            await voteService.ResetRoomVotes(Guid.Parse(roomId));
            await roomService.SwitchRoomState(Guid.Parse(roomId), RoomState.VotesHidden);
            await GetVotes(roomId);

            var updateResult = await userService.SwitchVotedStatus(Context.ConnectionId, false);
            if (!updateResult.Updated)
            {
                return;
            }
            await Clients.Group(roomId).SendAsync("UserVotedChanged", updateResult.UpdatedUser);

        }

        public async Task RevealVotes(string roomId)
        {
            var roomState = await roomService.SwitchRoomState(Guid.Parse(roomId));
            await Clients.Group(roomId).SendAsync("RoomStateChanged", roomState);

        }

        public async Task ChangeIsAVoterStatus(string roomId)
        {
            var updateResult = await userService.SwitchIsAVoterStatus(Context.ConnectionId);

            if (!updateResult.Updated)
            {
                return;
            }

            if (updateResult.UpdatedUser.IsAVoter)
            {
                await voteService.CreateVote(null, Context.ConnectionId, Guid.Parse(roomId));
            }
            else
            {
                await voteService.DeleteForUser(Guid.Parse(roomId), updateResult.UpdatedUser.Id);
            }

            await Clients.Client(Context.ConnectionId).SendAsync("UserStatusChanged", updateResult.UpdatedUser);
            await GetVotes(roomId);
        }
        public async Task ChangeVotedStatus(bool status)
        {
            var updateResult = await userService.SwitchVotedStatus(Context.ConnectionId, status);
            if (!updateResult.Updated)
            {
                return;
            }
            await Clients.Client(Context.ConnectionId).SendAsync("UserVotedChanged", updateResult.UpdatedUser);

        }

        public async Task ChangeRoomEstimationPack(string roomId, int estimationPackId)
        {
            await roomService.UpdateEstimationPack(Guid.Parse(roomId), estimationPackId);

            await Clients.Group(roomId).SendAsync("RoomEstimationPackChanged", estimationPackId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = await userService.GetUserByConnectionId(Context.ConnectionId);
                var roomId = Guid.Parse(Context.GetHttpContext().Request.Query["roomId"]);

                await voteService.DeleteForUser(roomId, user.Id);
                await userService.DeleteUser(Context.ConnectionId);

                await GetVotes(roomId.ToString());
            }
            finally
            {
                await base.OnDisconnectedAsync(exception);
            }
        }
    }
}
