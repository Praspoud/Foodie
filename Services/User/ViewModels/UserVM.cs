namespace Foodie.Services.User.ViewModels
{
    public class UserVM
    {
        public class LoginVM
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        //public class VerifyOtpVM
        //{
        //    public string Email { get; set; }
        //    public string OtpCode { get; set; }
        //}
        //public class ForgotPasswordVM
        //{
        //    public string Email { get; set; }
        //}
        //public class ChangePasswordVM
        //{
        //    public string OriginalPassword { get; set; }
        //    public string NewPassword { get; set; }
        //    public string ConfirmNewPassword { get; set; }
        //}
        public class UserRegistrationVM
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string EmailAddress { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
            //public string EmployeeCode { get; set; }
            //public int? partnerId { get; set; }
            //public string PartnerName { get; set; }
            //public int? positionId { get; set; }
            //public string? positionName { get; set; }
            //public bool IsActive { get; set; }

        }
    }
}
