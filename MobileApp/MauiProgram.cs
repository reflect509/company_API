using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MobileApp.Services;
using MobileApp.ViewModels;

namespace MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddTransient<NewsViewModel>();
        builder.Services.AddTransient<EventsViewModel>();

        return builder.Build();
    }
}
