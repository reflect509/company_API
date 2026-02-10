using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Desktop_app.Models
{
    public class Worker : INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> errors = new();

        public bool HasErrors => errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public int WorkerId { get; set; }
        private string _fullName;
        public required string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                ValidateFullName();
            }
        }
        public DateOnly? Birthdate { get; set; }
        private string? _phone;
        public required string? Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                ValidatePhone(nameof(Phone), _phone);
            }
        }
        private string? _workPhone;
        public string? WorkPhone
        {
            get => _workPhone;
            set
            {
                _workPhone = value;
                ValidatePhone(nameof(WorkPhone), _workPhone);
            }
        }        
        public required string Office { get; set; }
        private string _email;
        public required string Email
        {
            get => _email;
            set
            {
                _email = value;
                ValidateEmail();
            }
        }
        public bool? IsSubdepartmentHead { get; set; }
        public required string JobPosition { get; set; }
        public required int SubdepartmentId { get; set; }
        public string? Supervisor { get; set; }
        public string? SupervisorSupport { get; set; }
        // for UI
        public string? SubdepartmentName { get; set; }
        private void ValidateFullName()
        {
            ClearErrors(nameof(FullName));

            if (string.IsNullOrWhiteSpace(_fullName))
            {
                AddError(nameof(FullName), "ФИО не может быть пустым");
                return;
            }

            var parts = _fullName
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3)
                AddError(nameof(FullName), "ФИО должно содержать имя, фамилию и отчество");
        }
        private void ValidatePhone(string propertyName, string value)
        {
            ClearErrors(propertyName);

            if (string.IsNullOrWhiteSpace(value))
                return; // допустим пустое

            if (!Regex.IsMatch(value, @"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$"))
                AddError(propertyName, "Телефон должен быть в формате +7 (900) 123-45-67");
        }

        private void ValidateEmail()
        {
            ClearErrors(nameof(Email));

            if (string.IsNullOrWhiteSpace(_email))
                AddError(nameof(Email), "Поле email не может быть пустым");

            try
            {
                _ = new System.Net.Mail.MailAddress(_email);
            }
            catch
            {
                AddError(nameof(Email), "Некорректный email");
            }
        }       

        public IEnumerable GetErrors(string propertyName)
        {
            return propertyName != null && errors.ContainsKey(propertyName)
                ? errors[propertyName]
                : null;
        }

        private void AddError(string propertyName, string error)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            errors[propertyName].Add(error);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ClearErrors(string propertyName)
        {
            if (errors.Remove(propertyName))
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable<string> GetAllErrors()
        {
            return errors.SelectMany(e => e.Value);
        }

    }
}
