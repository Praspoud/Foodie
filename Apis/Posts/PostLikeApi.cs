using Foodie.Common.Models;
using Foodie.Services.Post;
using Foodie.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Foodie.Apis.Posts
{
    public static class PostLikeApi
    {
        public static void RegisterApi(this WebApplication app)
        {
            var root = "api/posts/{postId}/likes";
            app.MapPost(root, LikePost);
            app.MapDelete(root, UnLikePost);
            app.MapGet(root + "count", GetLikeCount);
            app.MapGet(root, IsPostLikedByUser);
        }

        private static IResult<int> LikePost(IPostLikeService service, IFoodieSessionAccessor accessor, int PostId)
        {
            return service.LikePost(accessor.UserId, PostId);
        }

        private static IResult<int> UnLikePost(IPostLikeService service, IFoodieSessionAccessor accessor, int PostId)
        {
            return service.UnLikePost(accessor.UserId, PostId);
        }

        private static IResult<int> GetLikeCount(IPostLikeService service, int PostId)
        {
            return service.GetLikeCount(PostId);
        }

        private static IResult<bool> IsPostLikedByUser(IPostLikeService service, IFoodieSessionAccessor accessor, int PostId)
        {
            return service.IsPostLikedByUser(accessor.UserId, PostId);
        }
    }
}
