using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FleetPro.WPF.ViewModels;

public partial class MainShellViewModel : ObservableObject
{
    private readonly VehiclesViewModel _vehiclesViewModel;

    private readonly VehicleFormViewModel _vehicleFormViewModel;

    [ObservableProperty]
    private ObservableObject? currentPage;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string role = string.Empty;

    public MainShellViewModel(
        VehiclesViewModel vehiclesViewModel,
        VehicleFormViewModel vehicleFormViewModel)
    {
        _vehiclesViewModel =
            vehiclesViewModel;

        _vehicleFormViewModel =
            vehicleFormViewModel;

        _vehicleFormViewModel.RequestClose +=
            OnVehicleFormRequestClose;

        _vehiclesViewModel.AddVehicleRequested +=
    OnAddVehicleRequested;
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
        CurrentPage =
            _vehiclesViewModel;

        await _vehiclesViewModel.LoadAsync();
    }

    [RelayCommand]
    private async Task AddVehicle()
    {
        await _vehicleFormViewModel.LoadAsync();

        CurrentPage =
            _vehicleFormViewModel;
    }

    private async void OnVehicleFormRequestClose(
        object? sender,
        EventArgs e)
    {
        await _vehiclesViewModel.LoadAsync();

        CurrentPage =
            _vehiclesViewModel;
    }

    private async void OnAddVehicleRequested(
    object? sender,
    EventArgs e)
{
    await _vehicleFormViewModel.LoadAsync();

    CurrentPage =
        _vehicleFormViewModel;
}
}