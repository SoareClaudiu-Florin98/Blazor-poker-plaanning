using BlazorPokerPlanning.Shared.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorPokerPlanning.Shared.Entities
{
    public class User : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid RoomId {get; set ; }

        public DateTime CreationDate { get; set; }

        public string ConnectionId { get; set; }

        public bool IsAVoter { get; set; }
        public bool Voted { get; set; }

    }
}
