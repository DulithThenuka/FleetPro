using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FleetPro.WPF.ViewModels;

public partial class MainShellViewModel : ObservableObject
{
    private readonly VehiclesViewModel _vehiclesViewModel;

    [ObservableProperty]
    private ObservableObject? currentPage;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string role = string.Empty;

    public MainShellViewModel(
        VehiclesViewModel vehiclesViewModel)
    {
        _vehiclesViewModel = vehiclesViewModel;
    }

    public void SetUser(
        string username,
        string role)
    {
        Username = username;
        Role = role;

        ShowDashboard();
    }

    [RelayCommand]
    private void ShowDashboard()
    {
        CurrentPage =
            new DashboardViewModel(
                Username,
                Role);
    }

    [RelayCommand]
    private async Task ShowVehicles()
    {
        CurrentPage = _vehiclesViewModel;

        await _vehiclesViewModel.LoadAsync();
    }
}