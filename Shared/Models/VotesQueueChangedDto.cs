using BlazorPokerPlanning.Shared.Entities;
using System.Collections.Generic;

namespace BlazorPokerPlanning.Shared.Models
{
    public class VotesQueueChangedDto
    {
        public List<Vote> Votes { get; set; }

        public RoomState RoomState { get; set; }
    }
}
