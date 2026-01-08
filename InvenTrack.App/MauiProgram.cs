using Inventrack.App;
using Inventrack.App.Services;
using Inventrack.App.Services.Http;
using Inventrack.App.Services.Interfaces;
using Inventrack.App.ViewModels;
using Inventrack.App.Views;

namespace Inventrack.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        builder.Services.AddSingleton<ISessionService, SessionService>();

        builder.Services.AddTransient<AuthHeaderHandler>();

        builder.Services.AddHttpClient("ApiClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7261/"); 
        })
        .AddHttpMessageHandler<AuthHeaderHandler>();

        // Services
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddTransient<IPackagesService, PackagesService>();

        // VMs
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MainViewModel>();

        // Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();

        // Shell
        builder.Services.AddSingleton<AppShell>();

        return builder.Build();
    }
}
