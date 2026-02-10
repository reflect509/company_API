using Desktop_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Desktop_app.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient httpClient;

        public ApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpClient.BaseAddress = new Uri("https://localhost:10020/api/v1/");
        }

        public async Task<IEnumerable<Node>> GetSubdepartmentsAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync("Subdepartments");
            if (response.IsSuccessStatusCode)
            {
                var subdepartments = await response.Content.ReadFromJsonAsync<IEnumerable<Node>>();
                return subdepartments;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
        public async Task<bool> UpdateWorker(Worker worker)
        {
            try
            {
                var json = JsonSerializer.Serialize(worker);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PatchAsync($"Workers/{worker.WorkerId}", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Можно логировать ошибку
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
