using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using Foodie.Common.Models;
using Foodie.Common.Services;
using Foodie.Models;
using Foodie.Services.Post.ViewModels;
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
        private readonly FileUpload _fileUpload;
        public UserPostService(IServiceFactory factory, FileUpload fileUpload)
        {
            _factory = factory;
            _userPostService = _factory.GetInstance<EUserPosts>();
            _userTagsService = _factory.GetInstance<EUserTags>();
            _hashTagsService = _factory.GetInstance<EHashTags>();
            _postHashTagsService = _factory.GetInstance<EPostHashTags>();
            _fileUpload = fileUpload;
        }

        public IResult<int> CreatePost(UserPostVM model, int UserId)
        {
            try
            {
                _factory.BeginTransaction();

                string mediaUrl = null;

                if (model.MediaFile != null)
                {
                    mediaUrl = _fileUpload.UploadFileAsync(model.MediaFile, "UserPostMedia").Result;
                }

                var post = new EUserPosts
                {
                    UserId = UserId,
                    Content = model.Content,
                    MediaUrl = mediaUrl,
                    CreatedAt = DateTime.UtcNow
                };
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
    }
}
