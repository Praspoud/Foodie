using Foodie.Common.Models;
using Foodie.Models;
using Foodie.Services.Post.ViewModels;

namespace Foodie.Services.Post
{
    public interface IPostReactionService
    {
        IResult<int> AddOrUpdateReaction(int userId, int postId, ReactionType reactionType);
        IResult<int> RemoveReaction(int userId, int postId);
        IResult<PostReactionSummaryVM> GetReactionCounts(int postId);
        IResult<bool> IsPostReactedByUser(int userId, int postId);
    }
}
