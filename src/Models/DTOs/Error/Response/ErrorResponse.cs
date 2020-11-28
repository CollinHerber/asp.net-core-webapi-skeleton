using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NetCoreExample.Server.Models.DTOs.Error.Response {
    public class ErrorResponse {
        public ErrorResponse(string message, ErrorReason reason = ErrorReason.InvalidData, string details = "") {
            Message = message;
            Reason = reason;
            Details = details;
        }

        public ErrorResponse(Dictionary<string, List<string>> validationErrors) {
            ValidationErrors = validationErrors;
            Reason = ErrorReason.ValidationError;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorReason Reason { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public Dictionary<string, List<string>> ValidationErrors { get; set; }

        public bool ShouldSerializeValidationErrors() {
            return ValidationErrors?.Count > 0;
        }

        public bool ShouldSerializeMessage() {
            return !string.IsNullOrWhiteSpace(Message);
        }

        public bool ShouldSerializeDetails() {
            return !string.IsNullOrWhiteSpace(Details);
        }
    }
}
