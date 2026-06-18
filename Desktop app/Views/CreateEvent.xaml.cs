using Desktop_app.Models;
using Desktop_app.Services;
using Desktop_app.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Desktop_app.Views
{
    public partial class CreateEvent : UserControl
    {
        public CreateEvent(IApiService apiService, Worker worker)
        {
            InitializeComponent();
            FillEventTypesCombobox(apiService, worker);
        }

        private void FillEventTypesCombobox(IApiService apiService, Worker worker)
        {
            var eventTypes = new ObservableCollection<string>
            {
                "Отпуск",
                "Отгул",
                "Больничный",
                "Обучение"
            };

            var viewModel = new CreateEventViewModel(apiService, worker)
            {
                EventTypes = eventTypes
            };

            DataContext = viewModel;
        }
    }
}
