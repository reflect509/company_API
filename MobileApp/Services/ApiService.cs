using MobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        public ApiService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://192.168.0.19:5000")
            };
        }

        public async Task<List<NewsItem>> GetNewsAsync()
        {
            var response = await _httpClient.GetStringAsync("/api/v1/News");
            return JsonConvert.DeserializeObject<List<NewsItem>>(response);
        }

        public async Task<List<EventItem>> GetEventsAsync()
        {
            var response = await _httpClient.GetStringAsync("/api/v1/Events");
            return JsonConvert.DeserializeObject<List<EventItem>>(response);
        }

        public async Task SubmitReactionAsync(string newsId, bool isPositive)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                NewsId = newsId,
                IsPositive = isPositive
            }), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync("/News/{NewsId}/Reaction", content);
        }
    }
}
