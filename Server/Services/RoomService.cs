using BlazorPokerPlanning.Shared.Entities;
using BlazorPokerPlanning.Shared.Interfaces;
using BlazorPokerPlanning.Shared.Models;
using System;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Server.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUserService userService;
        private readonly IRoomRepository roomRepository;

        public RoomService(IUserService userService, IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
            this.userService = userService;
        }

        public async Task<Room> CreateRoom(string connectionId, Guid newRoomId)
        {
            var user = await userService.CreateUser(newRoomId, connectionId);
            var newRoom = new Room(newRoomId, user);

            newRoom.Name = "Bla bla " + Guid.NewGuid();
            newRoom.CreationDate = DateTime.Now;
            newRoom.EstimationPackId = 1;
            newRoom.State = RoomState.VotesHidden;

            await roomRepository.InsertAsync(newRoom);

            return newRoom;
        }

        public async Task<Guid> GetRoomIdByConnectionId(string connectionId)
        {
            var connectedUser = await userService.GetUserByConnectionId(connectionId);
            if (connectedUser != null)
            {
                return connectedUser.RoomId;
            }
            return Guid.Empty;
        }

        public async Task<Room> GetRoomById(Guid roomId)
        {
            var room = await roomRepository.GetByIdAsync(roomId);

            return room;
        }

        public async Task AttachUserToRoom(Room room, User user)
        {
            room.Users.Add(user);
            await roomRepository.UpdateAsync(room);
        }

        public async Task<RoomState> SwitchRoomState(Guid roomId, RoomState? roomState = null)
        {
            var room = await roomRepository.GetByIdAsync(roomId);

            if (room == null)
            {
                return RoomState.VotesHidden;
            }

            room.State = roomState.GetValueOrDefault(room.State == RoomState.VotesHidden ? RoomState.VotesVisible : RoomState.VotesHidden);

            await roomRepository.UpdateAsync(room);

            return room.State;
        }

        public async Task UpdateEstimationPack(Guid roomId, int estimationPackId)
        {
            var room = await roomRepository.GetByIdAsync(roomId);

            room.EstimationPackId = estimationPackId;

            await roomRepository.UpdateAsync(room);
        }
    }
}
