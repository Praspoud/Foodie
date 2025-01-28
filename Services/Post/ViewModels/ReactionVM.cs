using Foodie.Models;

namespace Foodie.Services.Post.ViewModels
{
    public class ReactionVM
    {
        public int PostId { get; set; }
        public ReactionType ReactionType { get; set; }
    }

    public class PostReactionSummaryVM
    {
        public int PostId { get; set; }
        public Dictionary<ReactionType, int> ReactionCounts { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
