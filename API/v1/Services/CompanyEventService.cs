using API.v1.Models.DTOs;
using API.v1.Models;
using Microsoft.EntityFrameworkCore;

namespace API.v1.Services
{
    public class CompanyEventService : ICompanyEventService
    {
        private readonly ApiDbContext _dbContext;

        public CompanyEventService(ApiDbContext apiDbContext)
        {
            _dbContext = apiDbContext;
        }

        public async Task<List<EventDto>> GetEventsAsync()
        {
            return await _dbContext.CompanyEvents
                .AsNoTracking()
                .OrderByDescending(e => e.Date)
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date.HasValue ? e.Date.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                    Author = e.Author
                })
                .ToListAsync();
        }
    }
}
