using API.v1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace API.v1.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly ApiDbContext context;
        private readonly PasswordHasher<Worker> hasher;

        public WorkerService(ApiDbContext context)
        {
            this.context = context;
            this.hasher = new PasswordHasher<Worker>();
        }

        public async Task<Worker?> GetWorkerByIdAsync(int workerId)
        {
            return await context.Workers.FirstOrDefaultAsync(w => w.WorkerId == workerId);
        }

        public async Task<(string username, string password)> GenerateCredentialsAsync(int workerId)
        {
            var worker = await GetWorkerByIdAsync(workerId);
            if (worker == null)
                throw new ArgumentException($"Worker with id {workerId} not found");

            if (!string.IsNullOrEmpty(worker.UserName))
                throw new InvalidOperationException("Credentials already generated");

            // Генерация логина (можно менять по бизнес-логике)
            var fullName = worker.FullName.Split(' ');
            var surname = fullName[0];
            var name = fullName[1];
            var username = $"{surname.ToLower()}{name.ToLower()}";

            var bytes = new byte[9]; // 9 байт = 12 символов Base64
            RandomNumberGenerator.Fill(bytes); // криптографически безопасно
            var password = Convert.ToBase64String(bytes);
                
            // Хешируем пароль
            worker.UserPassword = hasher.HashPassword(worker, password);
            worker.UserName = username;

            await context.SaveChangesAsync();

            return (username, password); // вернуть оригинальный пароль один раз
        }
    }
}
