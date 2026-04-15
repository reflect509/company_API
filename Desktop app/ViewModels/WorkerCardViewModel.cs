using CommunityToolkit.Mvvm.Input;
using Desktop_app.Models;
using Desktop_app.Services;
using Desktop_app.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Desktop_app.ViewModels
{
    public class WorkerCardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IApiService apiService;
        public ICommand SaveWorkerCommand { get; }
        public ICommand CreateEventCommand { get; }
        private readonly Worker originalWorker;

        public WorkerCardViewModel(IApiService apiService, Worker selectedWorker, ObservableCollection<Worker> workers)
        {
            this.apiService = apiService;
            originalWorker = selectedWorker;
            LoadWorkers(selectedWorker, workers);            
            SaveWorkerCommand = new RelayCommand<Worker>(OnSaveChangesClicked);
            CreateEventCommand = new RelayCommand(OnCreateEventClicked);
        }

        private Worker selectedWorker;

        public Worker SelectedWorker
        {
            get { return selectedWorker; }
            set 
            {
                selectedWorker = value; 
                OnPropertyChanged(nameof(Worker));
            }
        }

        private ObservableCollection<Worker> workers;

        public ObservableCollection<Worker> Workers
        {
            get { return workers; } 
            set 
            { 
                workers = value; 
                OnPropertyChanged(nameof(Workers));
            }
        }

        private ObservableCollection<Node> subdepartments;

        public ObservableCollection<Node> Subdepartments
        {
            get { return subdepartments; }
            set 
            { 
                subdepartments = value;
                OnPropertyChanged(nameof(Subdepartments));
            }
        }
        private ObservableCollection<Event> events;

        public ObservableCollection<Event> Events
        {
            get { return events; }
            set
            {
                events = value; 
                OnPropertyChanged(nameof(Events));
            }
        }



        private async Task LoadWorkers(Worker selectedWorker, ObservableCollection<Worker> workers)
        {
            SelectedWorker = selectedWorker.Clone();
            Workers = new ObservableCollection<Worker>(workers);
            var subdepartments = await apiService.GetSubdepartmentsAsync();
            Subdepartments =  new ObservableCollection<Node>(subdepartments);
        }
        private async void OnSaveChangesClicked(Worker worker)
        {
            if (worker == null)
                return;
            if (worker.HasErrors)
            {
                var allErrors = worker
                    .GetAllErrors()
                    .Distinct();

                MessageBox.Show(
                    string.Join("\n", allErrors),
                           "Ошибка",
                   MessageBoxButton.OK,
                   MessageBoxImage.Warning);
                return;
            }
            try
            {                
                var result = await apiService.UpdateWorker(worker);

                if (result)
                {
                    originalWorker.CopyFrom(SelectedWorker);
                    MessageBox.Show("Данные успешно сохранены.");                    
                }                    
                else
                    MessageBox.Show("Ошибка сохранения данных.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
        public async Task LoadEventsAsync()
        {
            var events = await apiService.GetWorkerEventsAsync(SelectedWorker.WorkerId);

            Events.Clear();

            foreach (var ev in events)
                Events.Add(ev);
        }
        private async void OnCreateEventClicked()
        {
            var AddEventWindow = new CreateEvent(apiService, SelectedWorker.WorkerId);
            AddEventWindow.ShowDialog();
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
