using System.Diagnostics.Metrics;
using Foodie.Common.Models;
using Foodie.Common.Services;
using Foodie.Models;
using Foodie.Services.Post.ViewModels;
using Foodie.Services.Restaurant.ViewModels;
using Foodie.Services.User.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Foodie.Services.Post
{
    public class PostReactionService : IPostReactionService
    {
        private readonly IServiceFactory _factory;
        private readonly IServiceRepository<EPostReactions> _postReactService;

        public PostReactionService(IHttpContextAccessor httpContextAccessor, IServiceFactory factory, IConfiguration congf, FileUpload fileUpload)
        {
            _factory = factory;
            _postReactService = _factory.GetInstance<EPostReactions>();
        }

        public IResult<int> AddOrUpdateReaction(int userId, int postId, ReactionType reactionType)
        {
            try
            {
                var existingReaction = _postReactService.FindByName(l => l.UserId == userId && l.PostId == postId);
                if (existingReaction != null)
                {
                    existingReaction.ReactionType = reactionType;
                    var result = _postReactService.Update(existingReaction);

                    return new IResult<int>
                    {
                        Data = result.Id,
                        Message = "Post Reacted successfully.",
                        Status = ResultStatus.Success
                    };
                }
                else
                {
                    var newReaction = new EPostReactions
                    {
                        UserId = userId,
                        PostId = postId,
                        ReactionType = reactionType,
                        ReactedAt = DateTime.UtcNow
                    };
                    var result = _postReactService.Add(newReaction);

                    return new IResult<int>
                    {
                        Data = result.Id,
                        Message = "Post Reacted successfully.",
                        Status = ResultStatus.Success
                    };
                }
            }
            catch (Exception ex)
            {
                return new IResult<int>
                {
                    Message = "Post React Failed.",
                    Status = ResultStatus.Success
                };
            }
        }

        public IResult<int> RemoveReaction(int userId, int postId)
        {
            try
            {
                var reaction = _postReactService.FindByName(l => l.UserId == userId && l.PostId == postId);
                if (reaction == null)
                {
                    return new IResult<int>
                    {
                        Message = "No Post Reaction.",
                        Status = ResultStatus.Success
                    };
                }
                var result = _postReactService.Remove(reaction);

                return new IResult<int>
                {
                    Message = "Post UnReacted successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<int>
                {
                    Message = "Post React Failed.",
                    Status = ResultStatus.Success
                };
            }
        }

        public IResult<PostReactionSummaryVM> GetReactionCounts(int postId)
        {
            var reactions = _postReactService.List()
                .Where(r => r.PostId == postId)
                .GroupBy(r => r.ReactionType)
                .Select(g => new { ReactionType = g.Key, Count = g.Count() })
                .ToDictionary(k => k.ReactionType, v => v.Count);

            var count = _postReactService.List().Count(r => r.PostId == postId);

            return new IResult<PostReactionSummaryVM>
            {
                Data = new PostReactionSummaryVM
                {
                    PostId = postId,
                    ReactionCounts = reactions,
                    TotalCount = count
                },
                Message = "Reaction count retrieved successfully.",
                Status = ResultStatus.Success
            };
        }

        public IResult<bool> IsPostReactedByUser(int userId, int postId)
        {
            var reaction = _postReactService.List().Any(l => l.UserId == userId && l.PostId == postId);

            return new IResult<bool>
            {
                Data = reaction,
                Message = "Reaction retrieved successfully.",
                Status = ResultStatus.Success
            };
        }
    }
}
