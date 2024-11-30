using System.Security.Cryptography;
using Foodie.Common;
using System.Text.RegularExpressions;
using Foodie.Common.Services;
using Foodie.Models;
using Microsoft.VisualBasic;
using static Foodie.Services.User.ViewModels.UserVM;
using Microsoft.EntityFrameworkCore;

namespace Foodie.Services.User
{
    public class UserService : IUserService
    {
        private readonly IServiceFactory _factory;
        private readonly IServiceRepository<EUsers> _userService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IServiceRepository<EPasswordHist> _passwordHistService;
        public UserService(IHttpContextAccessor httpContextAccessor, IServiceFactory factory, IConfiguration congf)
        {
            _factory = factory;
            _userService = _factory.GetInstance<EUsers>();
            _configuration = congf;
            _httpContextAccessor = httpContextAccessor;
            _passwordHistService = _factory.GetInstance<EPasswordHist>();
        }

        public string GenerateOtpCode()
        {
            int otpLength = 6;

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[otpLength];
                rng.GetBytes(bytes);

                int number = BitConverter.ToInt32(bytes, 0) & int.MaxValue;
                int maxDigits = (int)Math.Pow(10, otpLength);
                int otpCode = number % maxDigits;
                return otpCode.ToString().PadLeft(otpLength, '0');
            }
        }

        public IResult<int> SignUp(UserRegistrationVM data)
        {
            _factory.BeginTransaction();
            try
            {
                //IServiceRepository<EUserStatusUpdate> userStatusService = _factory.GetInstance<EUserStatusUpdate>();
                //IServiceRepository<EProject> projectCRUD = _factory.GetInstance<EProject>();
                //var UserWithSameUserName = _userService.List().FirstOrDefault(x => x.UserName == data.UserName && x.Status == 0);
                var UserWithSameUserName = _userService.List().FirstOrDefault(x => x.UserName == data.UserName);

                if (UserWithSameUserName != null)
                {
                    return new IResult<int>
                    {
                        Data = 0,
                        Status = ResultStatus.Failure,
                        Message = "UserName Already Used"
                    };
                }
                //if (data.UserName.IsNullOrEmpty() || data.LoginName.IsNullOrEmpty() || data.EmployeeCode.IsNullOrEmpty())
                //{
                //    return new IResult<int>
                //    {
                //        Status = ResultStatus.Failure,
                //        Message = "Data Cannot Be Null"
                //    };
                //}

                if (string.IsNullOrEmpty(data.UserName))
                {
                    return new IResult<int>
                    {
                        Status = ResultStatus.Failure,
                        Message = "Data Cannot Be Null"
                    };
                }

                //var UserWithSameLoginName = _userService.List().FirstOrDefault(x => (x.LoginName == data.LoginName) && x.Status == 0);
                //if (UserWithSameLoginName != null)
                //{
                //    return new IResult<int>
                //    {
                //        Data = 0,
                //        Status = ResultStatus.Failure,
                //        Message = "LoginName Already Used"
                //    };
                //}
                //var UserWithSameEmail = _userService.List().FirstOrDefault(x => (x.EmailAddress == data.EmailAddress) && x.Status == 0);
                var UserWithSameEmail = _userService.List().FirstOrDefault(x => x.EmailAddress == data.EmailAddress);

                if (UserWithSameEmail != null)
                {
                    return new IResult<int>
                    {
                        Data = 0,
                        Status = ResultStatus.Failure,
                        Message = "Email already used by another user"
                    };
                }
                if (string.IsNullOrWhiteSpace(data.UserName))
                {
                    return new IResult<int>
                    {
                        Data = 0,
                        Status = ResultStatus.Failure,
                        Message = "No spaces allowed."
                    };
                }
                Regex regex = new Regex("[^a-zA-Z0-9]");
                if (regex.IsMatch(data.UserName))
                {
                    return new IResult<int>
                    {
                        Data = 0,
                        Status = ResultStatus.Failure,
                        Message = "No spaces or special characters allowed."
                    };
                }
                var user = new EUsers()
                {
                    Id = 0,
                    UserName = data.UserName,
                    EmailAddress = data.EmailAddress,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    PasswordHash = data.Password.ToHashString(),
                    TranDate = DateTime.Now,
                };
                var result = _userService.Add(user);
               // var generatedPassword = PasswordGenerator.GeneratePassword(8);
                var userPassword = new EPasswordHist()
                {
                    IsPasswordActive = true,
                    PasswordCreateDate = DateTime.Now,
                    PasswordText = data.Password.ToHashString(),
                    UserId = result.Id
                };
                _passwordHistService.Add(userPassword);
                //var StatusData = new EUserStatusUpdate()
                //{
                //    IsActive = true,
                //    StatusChangeUserId = userId,
                //    StatusUpdateDate = DateTime.Now,
                //    UserId = result.Id,
                //};
                //userStatusService.Add(StatusData);
                //var project = new EProject()
                //{
                //    ProjectName = $"MyProject_{data.LoginName}",
                //    ProjectType = projectType.PersonalProject,
                //    Status = 0,
                //    TranDate = DateTime.Now,
                //    TranUserId = result.Id.ToString(),
                //    DbType = null,
                //    StatusChangeUserId = userId.ToString(),
                //};
                //projectCRUD.Add(project);
                if (result.Id > 0)
                {
                    //string baseUrl = _configuration["FRONTEND:Url"];
                    //string htmlTemplate = System.IO.File.ReadAllText(TemplateConstant.USER_REGISTRATION_EMAIL_TEMPLATE);
                    //string body = string.Format(htmlTemplate, data.LoginName, generatedPassword ?? "", baseUrl);
                    //EmailService service = new EmailService(_configuration);
                    //var emailSend = false;
                    //if (!data.EmailAddress.IsNullOrEmpty())
                    //{
                    //    SentEmail("Registration Successful", body, data.EmailAddress);
                    //}
                    _factory.CommitTransaction();

                    return new IResult<int>
                    {
                        Data = result.Id,
                        Status = ResultStatus.Success,
                        Message = "User added successfully"
                    };
                }
                _factory.RollBack();

                return new IResult<int>
                {
                    Data = 0,
                    Status = ResultStatus.Failure,
                    Message = "Error While Adding User"
                };
            }
            catch (Exception ex)
            {
                _factory.RollBack();

                return new IResult<int>
                {
                    Data = 0,
                    Status = ResultStatus.Failure,
                    Message = "Some Error Occured"
                };
            }
        }

        public IResult<string> Login(LoginVM loginModel, IConfiguration config)
        {
            loginModel.Password = loginModel.Password.ToHashString();
            //IServiceRepository<EUserStatusUpdate> userStatusService = _factory.GetInstance<EUserStatusUpdate>();
            try
            {
                var user = _passwordHistService.List().Include(x => x.Users).FirstOrDefault(x => x.Users.UserName == loginModel.UserName && x.PasswordText == loginModel.Password && x.IsPasswordActive == true);

                if (user != null)
                {
                    //var userStatus = userStatusService.List().Include(x => x.User).FirstOrDefault(x => x.UserId == user.User.Id);

                    //if (userStatus == null)
                    //{
                    //    return new IResult<string>
                    //    {
                    //        Data = null,
                    //        Status = ResultStatus.Failure,
                    //        Message = "User Status Null."
                    //    };
                    //}
                    //if (userStatus.IsActive == true)
                    //{
                    //    PoweredUserType poweredType = GetPoweredType(user.User.Id);

                        string token = JWTBearer.CreateBearerToken(user.Users, config);

                        return new IResult<string>
                        {
                            Data = token,
                            Status = ResultStatus.Success,
                            Message = "Login Successful"
                        };
                    //else
                    //{
                    //    return new IResult<string>
                    //    {
                    //        Data = null,
                    //        Status = ResultStatus.Failure,
                    //        Message = "Please contact the administrator to activate your account."
                    //    };
                    //}
                }
                return new IResult<string>
                {
                    Data = null,
                    Status = ResultStatus.Failure,
                    Message = "Invalid UserName Or Password"
                };
            }
            catch (Exception ex)
            {
                return new IResult<string>
                {
                    Data = null,
                    Status = ResultStatus.Failure,
                    Message = "Some Error Occured"
                };
            }

        }
    }
}
