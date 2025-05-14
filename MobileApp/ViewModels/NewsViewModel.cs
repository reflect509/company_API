using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileApp.Models;
using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.ViewModels
{
    public partial class NewsViewModel : ObservableObject
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<NewsItem> news;

        public NewsViewModel()
        {
            _apiService = new ApiService();
            LoadNewsCommand = new AsyncRelayCommand(LoadNews);
            ShowReactionsCommand = new AsyncRelayCommand<NewsItem>(ShowReactions);
            LoadNewsCommand.Execute(null);
        }

        public IAsyncRelayCommand LoadNewsCommand { get; }
        public IAsyncRelayCommand<NewsItem> ShowReactionsCommand { get; }        

        private async Task LoadNews()
        {
            var items = await _apiService.GetNewsAsync();
            News = new ObservableCollection<NewsItem>(items);
        }

        private async Task ShowReactions(NewsItem item)
        {
            var result = await Shell.Current.DisplayActionSheet("Оцените новость", "Отмена", null, "👍", "👎");
            if (result == "👍")
                await _apiService.SubmitReactionAsync(item.Id, true);
            else if (result == "👎")
                await _apiService.SubmitReactionAsync(item.Id, false);

            await LoadNews(); // обновим количество реакций
        }
    }
}
