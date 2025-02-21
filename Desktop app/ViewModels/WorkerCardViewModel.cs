using Desktop_app.Models;
using Desktop_app.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_app.ViewModels
{
    public class WorkerCardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IApiService apiService;

        public WorkerCardViewModel(IApiService apiService, Worker selectedWorker, ObservableCollection<Worker> workers)
        {
            this.apiService = apiService;
            LoadWorkers(selectedWorker, workers);
        }

        private ObservableCollection<Worker> selectedWorker;

        public ObservableCollection<Worker> SelectedWorker
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

        private void LoadWorkers(Worker selectedWorker, ObservableCollection<Worker> workers)
        {
            SelectedWorker = new ObservableCollection<Worker>();
            SelectedWorker.Add(selectedWorker);
            Workers = new ObservableCollection<Worker>(workers);
            WorkerNames = new ObservableCollection<string>(workers.Select(w => w.FullName));
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
