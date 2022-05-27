using System;
using System.Collections.Generic;
using BlazorPokerPlanning.Shared.Entities;

namespace BlazorPokerPlanning.Shared.Models
{
    public class RoomStateDto
    {
        public RoomState RoomState { get; set; }

        public Guid RoomCreatedBy { get; set; }

        public User User { get; set; }

        public EstimationPack SelectedEstimationPack { get; set; }

        public IEnumerable<EstimationPack> EstimationPacks { get; set; }
    }
}