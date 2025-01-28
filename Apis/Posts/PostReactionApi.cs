using Foodie.Common.Models;
using Foodie.Models;
using Foodie.Services.Post;
using Foodie.Services.Post.ViewModels;
using Foodie.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Foodie.Apis.Posts
{
    public static class PostReactionApi
    {
        public static void RegisterApi(this WebApplication app)
        {
            var root = "api/posts/{postId}/reactions";
            app.MapPost(root, AddOrUpdateReaction);
            app.MapDelete(root, RemoveReaction);
            app.MapGet(root + "count", GetReactionCounts);
            app.MapGet(root, IsPostReactedByUser);
        }

        private static IResult<int> AddOrUpdateReaction(IPostReactionService service, IFoodieSessionAccessor accessor, int PostId, ReactionType reactionType)
        {
            return service.AddOrUpdateReaction(accessor.UserId, PostId, reactionType);
        }

        private static IResult<int> RemoveReaction(IPostReactionService service, IFoodieSessionAccessor accessor, int PostId)
        {
            return service.RemoveReaction(accessor.UserId, PostId);
        }

        private static IResult<PostReactionSummaryVM> GetReactionCounts(IPostReactionService service, int PostId)
        {
            return service.GetReactionCounts(PostId);
        }

        private static IResult<bool> IsPostReactedByUser(IPostReactionService service, IFoodieSessionAccessor accessor, int PostId)
        {
            return service.IsPostReactedByUser(accessor.UserId, PostId);
        }
    }
}
