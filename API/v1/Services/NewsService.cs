using API.v1.Models.DTOs;
using API.v1.Models;
using Microsoft.EntityFrameworkCore;

namespace API.v1.Services
{
    public class NewsService : INewsService
    {
        private readonly ApiDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor; // Added IHttpContextAccessor

        public NewsService(ApiDbContext apiDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = apiDbContext;
            _httpContextAccessor = httpContextAccessor; // Initialize IHttpContextAccessor
        }

        public async Task<List<NewsDto>> GetNewsAsync()
        {
            var request = _httpContextAccessor.HttpContext?.Request; // Access the current HTTP request

            return await _dbContext.News
                .AsNoTracking()
                .Select(n => new NewsDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    ImageUrl = request != null ? $"{request.Scheme}://{request.Host}/images/{n.ImageUrl}" : null, // Fixed 'Request' and 'news' issues
                    Date = n.Date.HasValue ? n.Date.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                    PositiveReactions = (int)n.PositiveReactions,
                    NegativeReactions = (int)n.NegativeReactions
                })
                .ToListAsync();
        }

        public async Task SubmitReactionAsync(int newsId, bool isPositive)
        {
            var news = await _dbContext.News.FindAsync(newsId)
                       ?? throw new KeyNotFoundException($"News {newsId} not found");

            if (isPositive) news.PositiveReactions++;
            else news.NegativeReactions++;

            await _dbContext.SaveChangesAsync();
        }
    }

}
