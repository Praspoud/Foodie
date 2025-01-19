using Foodie.Common.Models;
using Foodie.Services.Post;
using Foodie.Services.Post.ViewModels;
using Foodie.Services.User.ViewModels;
using Foodie.Utilities;

namespace Foodie.Apis.Posts
{
    public static class PostCommentApi
    {
        public static void RegisterApi(this WebApplication app)
        {
            var root = "api/posts/{postId}/comments/";
            app.MapPost(root, AddComment);
            app.MapPut(root, EditComment);
            app.MapDelete(root, DeleteComment);
            app.MapGet(root + "list/", GetCommentsForPost);
        }

        private static IResult<int> AddComment(IPostCommentService service, IFoodieSessionAccessor accessor, int postId, string content)
        {
            return service.AddComment(accessor.UserId, postId, content);
        }

        private static IResult<int> EditComment(IPostCommentService service, IFoodieSessionAccessor accessor, string newContent, int commentId)
        {
            return service.EditComment(commentId, accessor.UserId, newContent);
        }

        private static IResult<bool> DeleteComment(IPostCommentService service, IFoodieSessionAccessor accessor, int commentId)
        {
            return service.DeleteComment(accessor.UserId, commentId);
        }

        private static IResult<ListVM<PostCommentVM>> GetCommentsForPost(IPostCommentService service, int postId)
        {
            return service.GetCommentsForPost(postId);
        }
    }
}
