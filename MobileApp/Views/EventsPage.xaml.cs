using MobileApp.Models;
#if ANDROID
using MobileApp.Platforms.Android;
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
}
