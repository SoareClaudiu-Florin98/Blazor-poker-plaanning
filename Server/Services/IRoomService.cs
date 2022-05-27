using BlazorPokerPlanning.Shared.Entities;
using BlazorPokerPlanning.Shared.Models;
using System;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Server.Services
{
    public interface IRoomService
    {
        Task<Room> GetRoomById(Guid roomId);

        Task<Guid> GetRoomIdByConnectionId(string connectionId);

        Task<Room> CreateRoom(string connectionId, Guid roomId);

        Task AttachUserToRoom(Room room, User user);

        Task<RoomState> SwitchRoomState(Guid roomId, RoomState? roomState = null);

        Task UpdateEstimationPack(Guid roomId, int estimationPackId);
    }
}
