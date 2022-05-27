using System.Collections.Generic;

namespace BlazorPokerPlanning.Shared.Models
{
    public class EstimationPack
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> VoteOptions { get; set; }
    }
}