using System.Windows;
using FleetPro.Infrastructure;
using FleetPro.WPF.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FleetPro.Infrastructure.Data.Seed;
using FleetPro.Application.Services;
using FleetPro.Application.Interfaces;
using FleetPro.WPF.Views;

namespace FleetPro.WPF;

public partial class App : System.Windows.Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();

        ConfigureServices(services, configuration);

        _serviceProvider = services.BuildServiceProvider();
    }

    private static void ConfigureServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<LoginView>();

        services.AddScoped<AuthenticationService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        
    }

    protected override async void OnStartup(
    StartupEventArgs e)
{
    base.OnStartup(e);

    await DatabaseSeeder.SeedAsync(
        _serviceProvider);

    var mainWindow =
        _serviceProvider
        .GetRequiredService<MainWindow>();

    mainWindow.Show();
}
}