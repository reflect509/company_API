using CommunityToolkit.Mvvm.Input;
using Desktop_app.Models;
using Desktop_app.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public WorkerCardViewModel(IApiService apiService, Worker selectedWorker, ObservableCollection<Worker> workers)
        {
            this.apiService = apiService;
            LoadWorkers(selectedWorker, workers);
            SaveWorkerCommand = new RelayCommand<Worker>(OnSaveChangesClicked);
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

        private ObservableCollection<string> workerNames;

        public ObservableCollection<string> WorkerNames
        {
            get { return workerNames; }
            set
            {
                workerNames = value;
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


        private async Task LoadWorkers(Worker selectedWorker, ObservableCollection<Worker> workers)
        {
            SelectedWorker = selectedWorker;
            Workers = new ObservableCollection<Worker>(workers);
            WorkerNames = new ObservableCollection<string>(workers.Select(w => w.FullName));
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
                    MessageBox.Show("Данные успешно сохранены.");
                else
                    MessageBox.Show("Ошибка сохранения данных.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
