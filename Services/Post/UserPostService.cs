using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using Foodie.Common.Models;
using Foodie.Common.Services;
using Foodie.Models;
using Foodie.Services.Post.ViewModels;
using Foodie.Services.Restaurant.ViewModels;
using Foodie.Services.User.ViewModels;
using Lucene.Net.Support;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Foodie.Services.Post
{
    public class UserPostService : IUserPostService
    {
        private readonly IServiceFactory _factory;
        private readonly IServiceRepository<EUserPosts> _userPostService;
        private readonly IServiceRepository<EUserTags> _userTagsService;
        private readonly IServiceRepository<EHashTags> _hashTagsService;
        private readonly IServiceRepository<EPostHashTags> _postHashTagsService;
        private readonly IServiceRepository<ERestaurantRatings> _restaurantRatingService;
        private readonly FileUpload _fileUpload;
        public UserPostService(IServiceFactory factory, FileUpload fileUpload)
        {
            _factory = factory;
            _userPostService = _factory.GetInstance<EUserPosts>();
            _userTagsService = _factory.GetInstance<EUserTags>();
            _hashTagsService = _factory.GetInstance<EHashTags>();
            _postHashTagsService = _factory.GetInstance<EPostHashTags>();
            _restaurantRatingService = _factory.GetInstance<ERestaurantRatings>();
            _fileUpload = fileUpload;
        }

        public IResult<int> CreatePost(UserCreatePostVM model, int UserId)
        {
            try
            {
                _factory.BeginTransaction();

                //if (model.MediaFile != null)
                //{
                //    mediaUrl = _fileUpload.UploadFileAsync(model.MediaFile, "UserPostMedia").Result;
                //}

                var post = new EUserPosts
                {
                    UserId = UserId,
                    Content = model.Content,
                    CreatedAt = DateTime.UtcNow
                };

                if (model.MediaFiles != null)
                {
                    foreach (var file in model.MediaFiles)
                    {
                        var mediaUrl = _fileUpload.UploadFileAsync(file, "UserPostMedia").Result;
                        post.Media.Add(new EPostMedia
                        {
                            MediaUrl = mediaUrl,
                            MediaType = file.ContentType.Contains("image") ? "image" : "video"
                        });
                    }
                }

                var result = _userPostService.Add(post);

                if (model.TaggedUserIds?.Any() == true)
                {
                    var userTags = model.TaggedUserIds.Select(taggedUserId => new EUserTags
                    {
                        PostId = post.Id,
                        TaggedUserId = taggedUserId,
                        CreatedAt = DateTime.UtcNow
                    }).ToList();

                    _userTagsService.AddRange(userTags);
                }

                if (model.Hashtags?.Any() == true)
                {
                    foreach (var tag in model.Hashtags)
                    {
                        var hashtag = _hashTagsService.List().FirstOrDefault(h => h.Tag == tag);
                        if (hashtag == null)
                        {
                            hashtag = new EHashTags { Tag = tag };
                            _hashTagsService.Add(hashtag);
                        }

                        _postHashTagsService.Add(new EPostHashTags
                        {
                            PostId = post.Id,
                            HashTagId = hashtag.Id
                        });
                    }
                }

                if (model.RestaurantRating != 0 && model.RestaurantRatingType != null && model.RestaurantId != null)
                {
                    var rating =  new ERestaurantRatings
                    {
                        PostId = post.Id,
                        UserId = UserId,
                        RestaurantId = model.RestaurantId,
                        RatingType = model.RestaurantRatingType,
                        Score = model.RestaurantRating
                    };

                    _restaurantRatingService.Add(rating);
                }

                _factory.CommitTransaction();

                return new IResult<int>
                {
                    Data = result.Id,
                    Message = "Post created successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                _factory.RollBack();
                return new IResult<int>
                {
                    Status = ResultStatus.Failure,
                    Message = "Failed to create Post."
                };
            }
        }

        public IResult<UserPostVM> GetPostById(int postId)
        {
            try
            {
                var postLikes = _factory.GetInstance<EPostLikes>();
                var postComments = _factory.GetInstance<EPostComments>();

                var post = _userPostService.List().FirstOrDefault(p => p.Id == postId);
                var rating = _restaurantRatingService.List().FirstOrDefault(r => r.PostId == postId);

                if (post == null)
                {
                    return new IResult<UserPostVM>
                    {
                        Status = ResultStatus.Failure,
                        Message = "Failed to create UserPost."
                    };
                }

                var result = new UserPostVM
                {
                    Id = post.Id,
                    UserId = post.UserId,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt,
                    MediaUrls = post.Media.Select(m => m.MediaUrl).ToList(),
                    TaggedUsers = _userTagsService.List().Include(x => x.Users)
                            .Where(p => p.PostId == post.Id)
                            .Select(t => new TaggedUserVM
                            {
                                TaggedUserId = t.TaggedUserId,
                                TaggedUserName = t.Users.UserName
                            }).ToList(),
                    Hashtags = _postHashTagsService.List()
                            .Include(x => x.HashTags)
                            .Where(p => p.PostId == post.Id)
                            .Select(t => new HashTagVM
                            {
                                HashTagId = t.HashTagId,
                                HashTag = t.HashTags.Tag
                            }).ToList(),
                    RestaurantRating = rating == null ? null : rating.Score,
                    RestaurantRatingType = rating == null ? null : rating.RatingType,
                    RestaurantId = rating == null ? null : rating.RestaurantId,
                    LikesCount = postLikes.List().Count(l => l.PostId == postId),
                    CommentsCount = postComments.List().Count(l => l.PostId == postId),
                };

                return new IResult<UserPostVM>
                {
                    Data = result,
                    Message = "UserPost retrieved successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<UserPostVM>
                {
                    Message = "Failed to retrieve Post.",
                    Status = ResultStatus.Failure
                };
            }
        }

        public IResult<ListVM<UserPostVM>> GetUserPosts(int userId, int page, int pageSize)
        {
            try
            {
                var postLikes = _factory.GetInstance<EPostLikes>();
                var postComments = _factory.GetInstance<EPostComments>();

                var userPosts = _userPostService.List().Where(p => p.UserId == userId);
                var rating = _restaurantRatingService.List().Where(r => r.UserId == userId);

                var userPostList = userPosts.Include(p => p.Media)
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(userPosts => new UserPostVM
                    {
                        Id = userPosts.Id,
                        UserId = userPosts.UserId,
                        Content = userPosts.Content,
                        CreatedAt = userPosts.CreatedAt,
                        UpdatedAt = userPosts.UpdatedAt,
                        MediaUrls = userPosts.Media.Select(m => m.MediaUrl).ToList(),
                        TaggedUsers = _userTagsService.List().Include(x => x.Users)
                            .Where(p => p.PostId == userPosts.Id)
                            .Select(t => new TaggedUserVM
                            {
                                TaggedUserId = t.TaggedUserId,
                                TaggedUserName = t.Users.UserName
                            }).ToList(),
                        Hashtags = _postHashTagsService.List()
                            .Include(x => x.HashTags)
                            .Where(p => p.PostId == userPosts.Id)
                            .Select(t => new HashTagVM
                            {
                                HashTagId = t.HashTagId,
                                HashTag = t.HashTags.Tag
                            }).ToList(),
                        RestaurantRating = _restaurantRatingService.List().FirstOrDefault(r => r.PostId == userPosts.Id).Score == null 
                            ? null : _restaurantRatingService.List().FirstOrDefault(r => r.PostId == userPosts.Id).Score,
                        RestaurantRatingType = _restaurantRatingService.List().FirstOrDefault(r => r.PostId == userPosts.Id).RatingType == null
                            ? null : _restaurantRatingService.List().FirstOrDefault(r => r.PostId == userPosts.Id).RatingType,
                        RestaurantId = _restaurantRatingService.List().FirstOrDefault(r => r.PostId == userPosts.Id).RestaurantId == null
                            ? null : _restaurantRatingService.List().FirstOrDefault(r => r.PostId == userPosts.Id).RestaurantId,
                        LikesCount = postLikes.List().Count(l => l.PostId == userPosts.Id),
                        CommentsCount = postComments.List().Count(l => l.PostId == userPosts.Id)
                    }).ToList();

                return new IResult<ListVM<UserPostVM>>
                {
                    Data = new ListVM<UserPostVM>
                    {
                        List = userPostList,
                        Count = userPosts.Count()
                    },
                    Message = "UserPosts retrieved successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<ListVM<UserPostVM>>
                {
                    Message = "Failed to retrieve Posts.",
                    Status = ResultStatus.Failure
                };
            }
        }

        public IResult<int> UpdatePost(int postId, UserCreatePostVM model, int userId)
        {
            try
            {
                _factory.BeginTransaction();
                var post =  _userPostService.List().Include(p => p.Media).FirstOrDefault(p => p.Id == postId);

                if (post == null || post.UserId != userId)
                {
                    return new IResult<int>
                    {
                        Status = ResultStatus.Failure,
                        Message = "Can't update UserPost."
                    };
                }

                post.Content = model.Content;
                post.UpdatedAt = DateTime.UtcNow;

                if (model.MediaFiles != null)
                {
                    post.Media.Clear();
                    foreach (var file in model.MediaFiles)
                    {
                        var mediaUrl = _fileUpload.UploadFileAsync(file, "UserPostMedia").Result;
                        post.Media.Add(new EPostMedia
                        {
                            MediaUrl = mediaUrl,
                            MediaType = file.ContentType.Contains("image") ? "image" : "video"
                        });
                    }
                }
                var result = _userPostService.Update(post);

                if (model.TaggedUserIds?.Any() == true)
                {
                    var userTags = _userTagsService.List().Where(t => t.PostId == postId).ToList();
                    _userTagsService.RemoveRange(userTags);
                    var newUserTags = model.TaggedUserIds.Select(taggedUserId => new EUserTags
                    {
                        PostId = post.Id,
                        TaggedUserId = taggedUserId,
                        CreatedAt = DateTime.UtcNow
                    }).ToList();
                    _userTagsService.AddRange(newUserTags);
                }

                if (model.Hashtags?.Any() == true)
                {
                    var postHashTag = _postHashTagsService.List().Where(h => h.PostId == postId).ToList();
                    _postHashTagsService.RemoveRange(postHashTag);

                    foreach (var tag in model.Hashtags)
                    {
                        var hashtag = _hashTagsService.List().FirstOrDefault(h => h.Tag == tag);
                        if (hashtag == null)
                        {
                            hashtag = new EHashTags { Tag = tag };
                            _hashTagsService.Add(hashtag);
                        }

                        _postHashTagsService.Add(new EPostHashTags
                        {
                            PostId = post.Id,
                            HashTagId = hashtag.Id
                        });
                    }
                }

                if (model.RestaurantRating != 0 && model.RestaurantRatingType != null && model.RestaurantId != null)
                {
                    var restaurantRating = _restaurantRatingService.List().FirstOrDefault(r => r.PostId == postId);
                    _restaurantRatingService.Remove(restaurantRating);

                    var rating = new ERestaurantRatings
                    {
                        PostId = post.Id,
                        UserId = userId,
                        RestaurantId = model.RestaurantId,
                        RatingType = model.RestaurantRatingType,
                        Score = model.RestaurantRating
                    };

                    _restaurantRatingService.Add(rating);
                }
                _factory.CommitTransaction();

                return new IResult<int>
                {
                    Data = result.Id,
                    Status = ResultStatus.Success,
                    Message = "UserPost Updated Successfully"
                };
            }
            catch (Exception ex)
            {
                _factory.RollBack();
                return new IResult<int>
                {
                    Status = ResultStatus.Failure,
                    Message = "UserPost update failed."
                };
            }
        }

        public IResult<bool> DeletePost(int postId, int userId)
        {
            try
            {
                _factory.BeginTransaction();

                var post = _userPostService.List().FirstOrDefault(p => p.Id == postId);

                if (post == null || post.UserId != userId)
                {
                    return new IResult<bool>
                    {
                        Data = false,
                        Status = ResultStatus.Failure,
                        Message = "Can't Delete UserPost."
                    };
                }

                var result = _userPostService.Remove(post);
                _factory.CommitTransaction();

                return new IResult<bool>
                {
                    Data = true,
                    Status = ResultStatus.Success,
                    Message = "UserPost Deleted Successfully"
                };
            }
            catch (Exception ex) {
                _factory.RollBack();
                return new IResult<bool>
                {
                    Data = false,
                    Status = ResultStatus.Failure,
                    Message = "Can't Delete UserPost."
                };
            }
        }
    }
}
