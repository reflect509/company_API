using Desktop_app.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;

namespace Desktop_app.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient httpClient;

        public ApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpClient.BaseAddress = new Uri("http://localhost:5000");
        }           

        public async Task<List<Node>> GetSubdepartmentsAsync()
        {
            var response = await httpClient.GetStringAsync("/api/v1/Subdepartment");
            return JsonConvert.DeserializeObject<List<Node>>(response);
        }
    }
}
