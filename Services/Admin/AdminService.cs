using Foodie.Common.Models;
using Foodie.Common.Services;
using Foodie.Models;
using Microsoft.EntityFrameworkCore;
using static Foodie.Services.User.ViewModels.UserVM;

namespace Foodie.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IServiceFactory _factory;
        private readonly IConfiguration _configuration;
        private readonly IServiceRepository<EPasswordHist> _passwordHistService;

        public AdminService(IServiceFactory factory, IConfiguration congf)
        {
            _factory = factory;
            _configuration = congf;
            _passwordHistService = _factory.GetInstance<EPasswordHist>();
        }
        public IResult<string> AdminLogin(LoginVM loginModel, IConfiguration config)
        {
            loginModel.Password = loginModel.Password.ToHashString();

            try
            {
                var user = _passwordHistService.List().Include(x => x.Users).FirstOrDefault(x => x.Users.UserName == loginModel.UserName && x.PasswordText == loginModel.Password && x.IsPasswordActive == true);

                if (user != null)
                {
                    string token = JWTBearer.CreateBearerToken(user.Users, config);

                    return new IResult<string>
                    {
                        Data = token,
                        Status = ResultStatus.Success,
                        Message = "Login Successful"
                    };
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
