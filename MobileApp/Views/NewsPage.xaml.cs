using MobileApp.ViewModels;

namespace MobileApp.Views;

public partial class NewsPage : ContentPage
{
	public NewsPage()
	{
		InitializeComponent();
        BindingContext = new NewsViewModel();
    }
    void OnPrevClicked(object sender, EventArgs e)
        => carousel.ScrollTo(
            index: Math.Max(0, carousel.Position - 1),
            position: ScrollToPosition.Center);

    void OnNextClicked(object sender, EventArgs e)
        => carousel.ScrollTo(
            index: Math.Min(carousel.ItemsSource.Cast<object>().Count() - 1, carousel.Position + 1),
            position: ScrollToPosition.Center);
}