using System.Diagnostics.Eventing.Reader;
using Foodie.Common.Models;
using Foodie.Common.Services;
using Foodie.Models;
using Foodie.Services.User.ViewModels;
using Microsoft.Extensions.Hosting;

namespace Foodie.Services.User
{
    public class UserPostService : IUserPostService
    {
        private readonly IServiceFactory _factory;
        private readonly IServiceRepository<EUserPosts> _userPostService;
        private readonly FileUpload _fileUpload;
        public UserPostService(IServiceFactory factory, FileUpload fileUpload)
        {
            _factory = factory;
            _userPostService = _factory.GetInstance<EUserPosts>();
            _fileUpload = fileUpload;
        }

        public IResult<int> CreatePost(UserPostVM model, int UserId)
        {
            try
            {
                _factory.BeginTransaction();
                if (model.MediaFile != null)
                {
                    var mediaUrl = _fileUpload.UploadFileAsync(model.MediaFile, "UserPostMedia").Result;

                    var post = new EUserPosts
                    {
                        UserId = UserId,
                        Content = model.Content,
                        MediaUrl = mediaUrl,
                        CreatedAt = DateTime.UtcNow
                    };

                    var result = _userPostService.Add(post);
                    _factory.CommitTransaction();

                    return new IResult<int>
                    {
                        Data = result.Id,
                        Message = "Post created successfully.",
                        Status = ResultStatus.Success
                    };
                }
                else
                {
                    _factory.BeginTransaction();
                    var post = new EUserPosts
                    {
                        UserId = UserId,
                        Content = model.Content,
                        CreatedAt = DateTime.UtcNow
                    };
                    var result = _userPostService.Add(post);
                    _factory.CommitTransaction();

                    return new IResult<int>
                    {
                        Data = result.Id,
                        Message = "Post created successfully.",
                        Status = ResultStatus.Success
                    };
                }
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
