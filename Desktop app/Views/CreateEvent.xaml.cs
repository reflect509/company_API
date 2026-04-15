using Desktop_app.Models;
using Desktop_app.Services;
using Desktop_app.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Desktop_app.Views
{
    public partial class CreateEvent : Window
    {
        public CreateEvent(IApiService apiService, int workerId)
        {
            InitializeComponent();
            FillEventTypesCombobox(apiService, workerId);
        }

        private void FillEventTypesCombobox(IApiService apiService, int workerId)
        {
            var eventTypes = new ObservableCollection<string>
            {
                "Отпуск",
                "Отгул",
                "Больничный",
                "Обучение"
            };

            var viewModel = new CreateEventViewModel(apiService, workerId)
            {
                EventTypes = eventTypes
            };

            DataContext = viewModel;
        }
    }
}
