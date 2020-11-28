using NetCoreExample.Server.Models.Abstracts;
namespace NetCoreExample.Server.Models.DTOs.User.Response
{   
    public class UserEmployeeWithoutRelatedDto : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public float? EmailScore { get; set; }
        public string CharacterName { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool? Verified { get; set; }
        public bool EmailConfirmed { get; set; }
        public string AvatarImagePath { get; set; }
        public string PhoneNumber { get; set; }
        public int? PhoneCountryCode { get; set; }
        public long? AuthyId { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string AuthyOs { get; set; }
        public string AuthyCity { get; set; }
        public string AuthyCountry { get; set; }
        public string AuthyIp { get; set; }
        public string AuthyRegion { get; set; }
        public string AuthyRegistrationCity { get; set; }
        public string AuthyRegistrationCountry { get; set; }
        public string AuthyRegistrationDeviceId { get; set; }
        public string AuthyRegistrationIp { get; set; }
        public string AuthyRegistrationRegion { get; set; }
        public string AuthyRegistrationDate { get; set; }
        public long? VerifierId { get; set; }
    }
}
