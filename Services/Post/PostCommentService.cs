using Foodie.Common.Models;
using Foodie.Common.Services;
using Foodie.Models;
using Foodie.Services.Post.ViewModels;
using Foodie.Services.Restaurant.ViewModels;
using Foodie.Services.User.ViewModels;
using Microsoft.Extensions.Configuration;

namespace Foodie.Services.Post
{
    public class PostCommentService : IPostCommentService
    {
        private readonly IServiceFactory _factory;
        private readonly IServiceRepository<EPostComments> _postCommentService;

        public PostCommentService(IServiceFactory factory)
        {
            _factory = factory;
            _postCommentService = _factory.GetInstance<EPostComments>();
        }

        public IResult<int> AddComment(int userId, int postId, string content)
        {
            try
            {
                var comment = new EPostComments
                {
                    UserId = userId,
                    PostId = postId,
                    Content = content,
                    CreatedAt = DateTime.UtcNow
                };

                var result = _postCommentService.Add(comment);

                return new IResult<int>
                {
                    Data = result.Id,
                    Message = "Comment added successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<int>
                {
                    Message = "Comment Add Failed.",
                    Status = ResultStatus.Failure
                };
            }
        }

        public IResult<int> EditComment(int commentId, int userId, string newContent)
        {
            try
            {
                _factory.BeginTransaction();
                var comment = _postCommentService.FindByName(c => c.Id == commentId && c.UserId == userId);

                if (comment == null)
                {
                    return new IResult<int>
                    {
                        Message = "Comment does not exist.",
                        Status = ResultStatus.Failure
                    };
                }

                comment.Content = newContent;
                comment.UpdatedAt = DateTime.UtcNow;

                var result = _postCommentService.Update(comment);
                _factory.CommitTransaction();

                return new IResult<int>
                {
                    Data = result.Id,
                    Message = "Comment edited successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                _factory.RollBack();
                return new IResult<int>
                {
                    Status = ResultStatus.Failure,
                    Message = "Failed to edit Comment."
                };
            }
        }

        public IResult<bool> DeleteComment(int userId, int commentId)
        {
            try
            {
                var comment = _postCommentService.FindByName(c => c.Id == commentId && c.UserId == userId);

                if(comment == null)
                {
                    return new IResult<bool>
                    {
                        Data = false,
                        Message = "Comment does not exist.",
                        Status = ResultStatus.Failure
                    };
                }

                _postCommentService.Remove(comment);

                return new IResult<bool>
                {
                    Data = true,
                    Message = "Comment Deleted successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<bool>
                {
                    Message = "Comment Delete Failed.",
                    Status = ResultStatus.Failure
                };
            }
        }
        public IResult<ListVM<PostCommentVM>> GetCommentsForPost(int postId)
        {
            var comments = _postCommentService.List()
                .Where(c => c.PostId == postId);
            var commentsList = comments
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new PostCommentVM
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.Users.UserName, // Assuming User has UserName
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToList();

            return new IResult<ListVM<PostCommentVM>>
            {
                Data = new ListVM<PostCommentVM>
                {
                    List = commentsList,
                    Count = comments.Count()
                },
                Message = "Comments retrieved successfully.",
                Status = ResultStatus.Success
            };
        }

    }
}
