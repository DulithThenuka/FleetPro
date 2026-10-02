using System.Windows;
using FleetPro.Application.Interfaces;
using FleetPro.Application.Services;
using FleetPro.Infrastructure;
using FleetPro.Infrastructure.Data.Seed;
using FleetPro.WPF.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


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

        ConfigureServices(
            services,
            configuration);

        _serviceProvider =
            services.BuildServiceProvider();
    }

    private static void ConfigureServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddScoped<
            IAuthenticationService,
            AuthenticationService>();

        services.AddTransient<LoginViewModel>();

        services.AddSingleton<MainWindowViewModel>();

        services.AddSingleton<MainWindow>();

        services.AddScoped<
    IVehicleService,
    VehicleService>();

        services.AddTransient<VehiclesViewModel>();

        services.AddTransient<MainShellViewModel>();

        services.AddTransient<VehicleFormViewModel>();

        services.AddScoped<IDriverService, DriverService>();

        services.AddTransient<DriversViewModel>();
services.AddTransient<DriverFormViewModel>();
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

        var mainWindowViewModel =
            _serviceProvider
            .GetRequiredService<MainWindowViewModel>();

        var loginViewModel =
            _serviceProvider
            .GetRequiredService<LoginViewModel>();

        mainWindowViewModel
            .ShowLogin(loginViewModel);

        mainWindow.Show();
    }
}