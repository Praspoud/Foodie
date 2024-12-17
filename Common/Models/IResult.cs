namespace Foodie.Common.Models
{
    public class IResult<t>
    {
        public t? Data { get; set; }
        public ResultStatus Status { get; set; }
        public string? Message { get; set; }

    }
    public enum ResultStatus
    {
        Failure,
        Success
    }
}
