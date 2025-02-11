using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.v1.Models
{
    public class ApiError
    {
        public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public required string Message { get; set; }
        public int ErrorCode { get; set; } = 1000;
    }
}