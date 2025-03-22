namespace SalesManagement.API.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> Ok(T data, string message = null)
        {
            return new ApiResponse<T>(true, message ?? "Operation completed successfully", data);
        }

        public static ApiResponse<T> Error(string message, T data = default)
        {
            return new ApiResponse<T>(false, message, data);
        }
    }
}