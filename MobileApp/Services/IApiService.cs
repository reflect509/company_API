using MobilApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilApp.Services
{
    public interface IApiService
    {
        Task<List<NewsItem>> GetNewsAsync();
        Task<List<EventItem>> GetEventsAsync();
        Task SubmitReactionAsync(string newsId, bool isPositive);
    }
}
