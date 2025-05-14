using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileApp.Models;
#if ANDROID
using MobileApp.Platforms.Android;
#endif
using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.ViewModels
{
    public partial class EventsViewModel : ObservableObject
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<EventItem> events;

        public EventsViewModel()
        {
            _apiService = new ApiService();
            LoadEventsCommand = new AsyncRelayCommand(LoadEvents);
            LoadEventsCommand.Execute(null);
        }

        public IAsyncRelayCommand LoadEventsCommand { get; }
        public IAsyncRelayCommand<EventItem> AddToCalendarCommand { get; }

        private async Task LoadEvents()
        {
            var items = await _apiService.GetEventsAsync();
            Events = new ObservableCollection<EventItem>(items.OrderByDescending(e => e.Date));
        }
    }
}
