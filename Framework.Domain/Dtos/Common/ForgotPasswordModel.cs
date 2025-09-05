namespace Framework.Domain.Dtos.Common
{
    public class ForgotPasswordResponse
    {
        public string Email { get; set; }
    }

    public class ForgotPasswordResult
    {
        public string Message { get; set; }
    }

    public class ValidateTokenPasswordResponse
    {
        public string Token { get; set; }
    }

    public class ValidateTokenPasswordResult
    {
        public string Email { get; set; }
    }

    public class ChangePasswordResponse
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangePasswordResult
    {
        public string Message { get; set; }
    }

    public class ChangePasswordByForgotResponse
    {
        public string Guid { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangePasswordByForgotResult
    {
        public string Message { get; set; }
    }
}
