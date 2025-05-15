using API.v1.Models.DTOs;

namespace API.v1.Services
{
    public interface INewsService
    {
        Task<List<NewsDto>> GetNewsAsync();
        Task SubmitReactionAsync(int newsId, bool isPositive);
    }
}
