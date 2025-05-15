using CommunityToolkit.Mvvm.Input;

namespace MobileApp
{
    public partial class AppShell : Shell
    {
        public IRelayCommand<string> NavigateCommand { get; }
        public AppShell()
        {
            InitializeComponent();
            NavigateCommand = new RelayCommand<string>(async route =>
            {
                Background = Colors.Yellow;
                await GoToAsync($"//{route}");
            });
            BindingContext = this;
        }
    }
}
