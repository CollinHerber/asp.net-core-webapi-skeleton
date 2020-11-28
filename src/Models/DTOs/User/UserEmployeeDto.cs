using NetCoreExample.Server.Models.Abstracts;
using System.Collections.Generic;

namespace NetCoreExample.Server.Models
{   
    public class UserEmployeeDto : BaseEntity
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
        public bool PhoneNumberConfirmed { get; set; }
        public long? VerifierId { get; set; }
        public List<string> Roles { get; set; }
    }
}
