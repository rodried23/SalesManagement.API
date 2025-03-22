namespace SalesManagement.API.Models
{
    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Error { get; set; }
        public string Detail { get; set; }
    }
}