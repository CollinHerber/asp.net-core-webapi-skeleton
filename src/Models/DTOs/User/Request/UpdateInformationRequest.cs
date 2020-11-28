using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreExample.Server.Models.DTOs.User.Request {
    public class UpdateInformationRequest {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CharacterName { get; set; }
        public string Email { get; set; }
        public string AvatarImagePath { get; set; }
        public long? PaymentMethodId { get; set; }
        public string DiscordId { get; set; }
        public bool OptedInForEmails { get; set; }
    }
}
