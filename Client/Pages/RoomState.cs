using System;
using System.Collections.Generic;
using BlazorPokerPlanning.Shared.Entities;
using BlazorPokerPlanning.Shared.Models;

namespace BlazorPokerPlanning.Client.Pages
{
    public class RoomState
    {
        public RoomState()
        {
            Votes = new List<Vote>();
            User = new User();
        }

        public Guid RoomCreatedBy { get; set; }

        public BlazorPokerPlanning.Shared.Models.RoomState State { get; set; }

        public List<Vote> Votes { get; set; }

        public User User { get; set; }

        public EstimationPack SelectedEstimationPack { get; set; }

        public IEnumerable<EstimationPack> EstimationPacks { get; set; }
    }
}