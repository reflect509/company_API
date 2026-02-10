using API.v1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

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

        public async Task<(bool Success, string Error)> UpdateWorkerAsync(Worker worker)
        {
            var existingWorker = await context.Workers.FindAsync(worker.WorkerId);
            if (existingWorker == null)
                return (false, "Работник не найден");

            if (!IsValidFullName(worker.FullName))
                return (false, "ФИО должно состоять из трёх слов");

            if (!IsValidPhone(worker.Phone) || !IsValidPhone(worker.WorkPhone))
                return (false, "Телефон имеет неверный формат");

            if (!IsValidEmail(worker.Email))
                return (false, "Некорректный email");

            existingWorker.FullName = worker.FullName;
            existingWorker.Phone = worker.Phone;
            existingWorker.WorkPhone = worker.WorkPhone;
            existingWorker.Email = worker.Email;
            existingWorker.JobPosition = worker.JobPosition;
            existingWorker.SubdepartmentId = worker.SubdepartmentId;
            existingWorker.Office = worker.Office;
            existingWorker.Supervisor = worker.Supervisor;

            await context.SaveChangesAsync();

            return (true, null);
        }

        private static bool IsValidFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return false;

            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length == 3;
        }

        private static bool IsValidPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return true;

            var regex = new Regex(@"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$");
            return regex.IsMatch(phone);
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
