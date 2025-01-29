using Foodie.Common.Models;
using Foodie.Services.Post.ViewModels;

namespace Foodie.Services.Post
{
    public interface IPostCommentService
    {
        IResult<int> AddComment(int userId, int postId, int? parentCommentId, string content);
        IResult<int> EditComment(int commentId, int userId, string newContent);
        IResult<bool> DeleteComment(int userId, int commentId);
        IResult<ListVM<PostCommentVM>> GetCommentsForPost(int postId);
        IResult<ListVM<PostCommentVM>> GetCommentRepliesForPost(int commentId);
        IResult<bool> ReportComment(int commentId, int userId);
    }
}
