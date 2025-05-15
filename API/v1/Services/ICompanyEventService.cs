using API.v1.Models.DTOs;

namespace API.v1.Services
{
    public interface ICompanyEventService
    {
        Task<List<EventDto>> GetEventsAsync();
    }
}
