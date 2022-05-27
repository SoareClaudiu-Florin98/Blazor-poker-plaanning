using BlazorPokerPlanning.Shared.Interfaces;
using BlazorPokerPlanning.Shared.Models;
using System;
using System.Collections.Generic;

namespace BlazorPokerPlanning.Shared.Entities
{
    public class Room : IEntity
    {
        public Room(Guid id, User admin)
        {
            Users = new List<User> { admin };
            CreatedByUserId = admin.Id;
            Id = id;
        }

        internal Room()
        {
        }

        public Guid Id { get; private set; }
        public string Name { get; set; }
        public DateTime CreationDate {get ; set; }
        public Guid CreatedByUserId { get; private set; }
        public int EstimationPackId { get; set; }
        public RoomState State { get; set; }
        public ICollection<User> Users { get; private set; }
    }
}
