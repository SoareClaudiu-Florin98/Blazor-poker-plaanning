using System.Collections.Generic;
using BlazorPokerPlanning.Shared.Models;

namespace BlazorPokerPlanning.Server.Services
{
    public interface IEstimationPackService
    {
        IEnumerable<EstimationPack> GetAll();
    }

    public class EstimationPackService : IEstimationPackService
    {
        private static IEnumerable<EstimationPack> estimationPacks = new List<EstimationPack>
        {
            new() { Id = 1, Name = "Mountain Goat", VoteOptions = new[] { "0", "½", "1", "2", "3", "5", "8", "13", "20", "40", "100", "?", "☕" } },
            new() { Id = 2, Name = "Fibonacci", VoteOptions = new[] { "0", "1", "2", "3", "5", "8", "13", "21", "34", "55", "89", "?", "☕" } },
            new() { Id = 3, Name = "Sequential", VoteOptions = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "?", "☕" } },
            new() { Id = 4, Name = "Playing cards", VoteOptions = new[] { "A♠", "2", "3", "5", "8", "♔", "?", "☕" } },
            new() { Id = 5, Name = "T-Shirt", VoteOptions = new[] { "XS", "S", "M", "L", "XL", "?", "☕" } }
        };

        public IEnumerable<EstimationPack> GetAll()
        {
            return estimationPacks;
        }
    }
}