namespace NetCoreExample.Server.Models.DTOs.User.Request
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CharacterName {get; set;}
        public string Token { get; set; }
        public bool OptedInForEmails { get; set; }
        public string Ip { get; set; }
        public string WebsiteShortCode { get; set; }
    }
}
