using Android.Content;
using Android.Provider;
using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Platforms.Android
{
    public static class CalendarInterop
    {
        public static void AddEventToCalendar(EventItem ev)
        {
            var context = Platform.CurrentActivity ?? throw new InvalidOperationException("No current activity");

            Intent intent = new Intent(Intent.ActionInsert)
                .SetData(CalendarContract.Events.ContentUri)
                .PutExtra(CalendarContract.Events.InterfaceConsts.Title, ev.Title)
                .PutExtra(CalendarContract.Events.InterfaceConsts.Description, ev.Description)
                .PutExtra(CalendarContract.Events.InterfaceConsts.Dtstart, GetUnixMillis(ev.Date))
                .PutExtra(CalendarContract.Events.InterfaceConsts.Dtend, GetUnixMillis(ev.Date.AddHours(1)))
                .PutExtra(CalendarContract.Events.InterfaceConsts.AllDay, true);

            context.StartActivity(intent);
        }

        private static long GetUnixMillis(DateTime dateTime)
        {
            return (long)(TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}
