using MobileApp.Models;
#if ANDROID
using MobileApp.Platforms.Android;
using MobileApp.ViewModels;
using System.Collections;
#endif
namespace MobileApp.Views;

public partial class EventsPage : ContentPage
{
    public EventsPage()
    {
        InitializeComponent();
    }

    private void OnAddToCalendarClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is EventItem ev)
        {
#if ANDROID
            CalendarInterop.AddEventToCalendar(ev); // Ensure CalendarInterop is implemented in the Android namespace.    
#else
           Application.Current.MainPage.DisplayAlert("Календарь", "Добавление доступно только на Android.", "OK");  
#endif
        }
    }

    private void OnUpClicked(object sender, EventArgs e)
    {
        if (EventsList.ItemsSource is IList<object> items && items.Count > 0)
            EventsList.ScrollTo(items[0], position: ScrollToPosition.Start, animate: true);
    }

    private void OnDownClicked(object sender, EventArgs e)
    {
        if (EventsList.ItemsSource is IList<object> items && items.Count > 0)
            EventsList.ScrollTo(items[^1], position: ScrollToPosition.End, animate: true);
    }
}
