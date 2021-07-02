namespace Admin.Models
{
    public class UserForLogin
    {
        public string EmailOrNickname { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}