using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Services
{
    public interface IApiService
    {
        Task<List<NewsItem>> GetNewsAsync();
        Task<List<EventItem>> GetEventsAsync();
        Task SubmitReactionAsync(string newsId, bool isPositive);
    }
}
