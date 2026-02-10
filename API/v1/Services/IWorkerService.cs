using API.v1.Models;

namespace API.v1.Services
{
    public interface IWorkerService
    {
        Task<Worker?> GetWorkerByIdAsync(int workerId);
        Task<(string username, string password)> GenerateCredentialsAsync(int workerId);
        Task<(bool Success, string Error)> UpdateWorkerAsync(Worker worker);
    }
}
