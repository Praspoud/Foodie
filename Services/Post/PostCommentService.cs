using System;
using Foodie.Common.Models;
using Foodie.Common.Services;
using Foodie.Models;
using Foodie.Services.Post.ViewModels;
using Foodie.Services.Restaurant.ViewModels;
using Foodie.Services.User.ViewModels;
using Foodie.Utilities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Foodie.Services.Post
{
    public class PostCommentService : IPostCommentService
    {
        private readonly IServiceFactory _factory;
        private readonly IServiceRepository<EPostComments> _postCommentService;
        private readonly IServiceRepository<EUsers> _userService;
        private readonly IServiceRepository<EUserMentions> _userMentionService;

        public PostCommentService(IServiceFactory factory)
        {
            _factory = factory;
            _postCommentService = _factory.GetInstance<EPostComments>();
            _userService = _factory.GetInstance<EUsers>();
            _userMentionService = _factory.GetInstance<EUserMentions>();
        }

        public IResult<int> AddComment(int userId, int postId, int? parentCommentId, string content)
        {
            try
            {
                var comment = new EPostComments
                {
                    UserId = userId,
                    PostId = postId,
                    Content = content,
                    ParentCommentId = parentCommentId,
                    CreatedAt = DateTime.UtcNow
                };

                var result = _postCommentService.Add(comment);

                var mentionedUsernames = MentionExtractor.ExtractMentions(content);
                var mentionedUsers = _userService.List()
                    .Where(u => mentionedUsernames.Contains(u.UserName))
                    .ToList();

                foreach (var user in mentionedUsers)
                {
                    _userMentionService.Add(new EUserMentions
                    {
                        CommentId = comment.Id,
                        MentionedUserId = user.Id
                    });

                    //// Notify mentioned user
                    //await _hubContext.Clients.User(user.Id.ToString())
                    //    .SendAsync("ReceiveMentionNotification", comment.PostId, userId, model.Content);
                }

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

                if ((DateTime.UtcNow - comment.CreatedAt).TotalMinutes > 10)
                {
                    return new IResult<int>
                    {
                        Message = "Comment edit time exceeded.",
                        Status = ResultStatus.Failure
                    };
                }

                comment.Content = newContent;
                comment.UpdatedAt = DateTime.UtcNow;

                var result = _postCommentService.Update(comment);

                var mentionedUsernames = MentionExtractor.ExtractMentions(newContent);
                var mentionedUsers = _userService.List()
                    .Where(u => mentionedUsernames.Contains(u.UserName))
                    .ToList();
                if (mentionedUsers.Any())
                {
                    var mentions = _userMentionService.List().Where(m => m.CommentId == commentId).ToList();
                    _userMentionService.RemoveRange(mentions);

                    foreach (var user in mentionedUsers)
                    {
                        _userMentionService.Add(new EUserMentions
                        {
                            CommentId = comment.Id,
                            MentionedUserId = user.Id
                        });

                        //// Notify mentioned user
                        //await _hubContext.Clients.User(user.Id.ToString())
                        //    .SendAsync("ReceiveMentionNotification", comment.PostId, userId, model.Content);
                    }
                }
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
                .Where(c => c.PostId == postId && c.ParentCommentId == null);
            var commentsList = comments
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new PostCommentVM
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.Users.UserName, // Assuming User has UserName
                    Content = c.Content,
                    MentionedUsers = _userMentionService.List()
                        .Where(m => m.CommentId == c.Id)
                        .Select(m => new MentionedUserVM
                        {
                            MentionedUserId = m.MentionedUserId,
                            MentionedUserName = m.MentionedUsers.UserName,
                        }).ToList(),
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

        public IResult<ListVM<PostCommentVM>> GetCommentRepliesForPost(int commentId)
        {
            var comments = _postCommentService.List()
                .Where(c => c.ParentCommentId == commentId);
            var commentsList = comments
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new PostCommentVM
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.Users.UserName, // Assuming User has UserName
                    Content = c.Content,
                    MentionedUsers = _userMentionService.List()
                        .Where(m => m.CommentId == c.Id)
                        .Select(m => new MentionedUserVM
                        {
                            MentionedUserId = m.MentionedUserId,
                            MentionedUserName = m.MentionedUsers.UserName,
                        }).ToList(),
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
                Message = "Replies retrieved successfully.",
                Status = ResultStatus.Success
            };
        }

        public IResult<bool> ReportComment(int commentId, int userId)
        {
            try
            {
                _factory.BeginTransaction();
                var comment = _postCommentService.List().FirstOrDefault(c => c.Id == commentId);
                if (comment == null)
                {
                    return new IResult<bool>
                    {
                        Data = false,
                        Message = "Comment does not exist.",
                        Status = ResultStatus.Failure
                    };
                }

                comment.IsReported = true;
                comment.ModerationStatus = "Pending";

                var result = _postCommentService.Update(comment);
                _factory.CommitTransaction();
                return new IResult<bool>
                {
                    Data = true,
                    Message = "Comment reported successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                _factory.RollBack();
                return new IResult<bool>
                {
                    Data = false,
                    Status = ResultStatus.Failure,
                    Message = "Failed to report Comment."
                };
            }
        }
    }
}
