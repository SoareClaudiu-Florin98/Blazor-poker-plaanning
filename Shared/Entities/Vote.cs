using BlazorPokerPlanning.Shared.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorPokerPlanning.Shared.Entities
{
    public class Vote : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public string Value { get; set; }
    }
}
