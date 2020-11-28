namespace NetCoreExample.Server.Models.DTOs.User.Response
{
    public class OrderCustomerInfoResponse
    {
        public long Id {get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public float? EmailScore { get; set; }
        public string CharacterName { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneCountryCode { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }
}
