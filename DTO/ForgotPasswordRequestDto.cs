namespace HomyWayAPI.DTO
{
    public class ForgotPasswordRequestDto
    {
        public string Phone { get; set; }
    }

    public class VerifyOtpDto
    {
        public string Phone { get; set; }
        public string Otp { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Phone { get; set; }
        public string NewPassword { get; set; }
    }
}
