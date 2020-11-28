using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace NetCoreExample.Server.Models
{
    [Table("User")]
    public class User : IdentityUser<long>
    {
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CharacterName { get; set; }
        public bool? Verified { get; set; }
        public string AvatarImagePath { get; set; }
        public string TwoFactorSecret { get; set; }
        public int? PhoneCountryCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool OptedInForEmails { get; set; }
        public bool IsDeleted { get; set; }
        public string LastLoggedInIp { get; set; }
    }
}
