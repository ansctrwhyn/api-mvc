namespace API.Models.ViewModel
{
    public class ChangePasswordVM
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int OTP { get; set; }
    }
}
