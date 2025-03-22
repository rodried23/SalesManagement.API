using System.Collections.Generic;

namespace SalesManagement.API.Models
{
    public class ValidationErrorResponse : ErrorResponse
    {
        public IEnumerable<ValidationError> Errors { get; set; }

        public ValidationErrorResponse()
        {
            Type = "ValidationError";
            Error = "Validation error";
            Detail = "One or more validation errors occurred";
            Errors = new List<ValidationError>();
        }
    }

    public class ValidationError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}