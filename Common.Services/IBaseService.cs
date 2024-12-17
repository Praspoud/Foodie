using Foodie.Common.Models;

namespace Foodie.Common.Services
{
    public abstract class IBaseService
    {
        protected IResult<t> ProcessResult<t>(t Data, string message = "", ResultStatus status = ResultStatus.Success)
        {
            return new IResult<t>()
            {
                Data = Data,
                Message = message,
                Status = status
            };
        }
        protected IResult<t> Success<t>(t Data, string message = "")
        {
            return new IResult<t>()
            {
                Data = Data,
                Message = message,
                Status = ResultStatus.Success
            };
        }
        protected IResult<t> Failure<t>(string message = "")
        {
            return new IResult<t>()
            {

                Message = message,
                Status = ResultStatus.Failure
            };
        }
    }
}
