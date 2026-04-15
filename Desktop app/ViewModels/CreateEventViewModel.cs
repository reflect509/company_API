using CommunityToolkit.Mvvm.Input;
using Desktop_app.Models;
using Desktop_app.Services;
using Desktop_app.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Desktop_app.ViewModels
{
    public class CreateEventViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IApiService apiService;
        private readonly int workerId;
        private ObservableCollection<string> eventTypes;
        private string selectedEventType;
        private DateTime? eventDate;
        private string description;

        public ObservableCollection<string> EventTypes
        {
            get { return eventTypes; }
            set
            {
                eventTypes = value;
                OnPropertyChanged(nameof(EventTypes));
            }
        }

        public string SelectedEventType
        {
            get { return selectedEventType; }
            set
            {
                selectedEventType = value;
                OnPropertyChanged(nameof(SelectedEventType));
            }
        }

        public DateTime? EventDate
        {
            get { return eventDate; }
            set
            {
                eventDate = value;
                OnPropertyChanged(nameof(EventDate));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public CreateEventViewModel(IApiService apiService, int workerId)
        {
            this.apiService = apiService;
            this.workerId = workerId;

            SaveCommand = new RelayCommand(OnSaveClicked);
            CancelCommand = new RelayCommand(OnCancelClicked);
        }

        public CreateEventViewModel()
        {
            SaveCommand = new RelayCommand(OnSaveClicked);
            CancelCommand = new RelayCommand(OnCancelClicked);
        }

        private async void OnSaveClicked()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SelectedEventType) || !EventDate.HasValue)
                {
                    MessageBox.Show("Заполните все обязательные поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newEvent = new Event
                {
                    EventType = SelectedEventType,
                    Date = EventDate.Value,
                    Description = Description,
                    EventName = SelectedEventType
                };

                await apiService.CreateWorkerEventAsync(workerId, newEvent);

                MessageBox.Show("Событие успешно создано.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseWindow();
            }
            catch (Exception ex)
            {
                string errorMessage = GetErrorMessage(ex);
                MessageBox.Show($"Ошибка при создании события: {errorMessage}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetErrorMessage(Exception ex)
        {
            if (ex == null)
                return "Неизвестная ошибка";

            try
            {
                return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(ex.Message));
            }
            catch
            {
                return ex.GetType().Name;
            }
        }

        private void OnCancelClicked()
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<CreateEvent>().FirstOrDefault()?.Close();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}