using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FleetPro.Application.Interfaces;

namespace FleetPro.WPF.ViewModels;

public partial class MainShellViewModel : ObservableObject
{
    private readonly VehiclesViewModel _vehiclesViewModel;

    private readonly VehicleFormViewModel _vehicleFormViewModel;

    private readonly IVehicleService _vehicleService;

    [ObservableProperty]
    private ObservableObject? currentPage;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string role = string.Empty;

    private readonly DriversViewModel _driversViewModel;
private readonly DriverFormViewModel _driverFormViewModel;

private readonly VehicleAssignmentViewModel _vehicleAssignmentViewModel;

    public MainShellViewModel(
    VehiclesViewModel vehiclesViewModel,
    VehicleFormViewModel vehicleFormViewModel,
    IVehicleService vehicleService,
    DriversViewModel driversViewModel,
    DriverFormViewModel driverFormViewModel,
    VehicleAssignmentViewModel vehicleAssignmentViewModel)
{
    _vehiclesViewModel =
        vehiclesViewModel;

    _vehicleFormViewModel =
        vehicleFormViewModel;

    _vehicleService =
        vehicleService;

    _vehicleAssignmentViewModel = vehicleAssignmentViewModel;

    _vehicleFormViewModel.RequestClose +=
        OnVehicleFormRequestClose;

    _vehiclesViewModel.AddVehicleRequested +=
        OnAddVehicleRequested;

    _vehiclesViewModel.EditVehicleRequested +=
        OnEditVehicleRequested;

        _driversViewModel = driversViewModel;
_driverFormViewModel = driverFormViewModel;

_driversViewModel.AddDriverRequested += OnAddDriverRequested;

_driversViewModel.AssignVehicleRequested += OnAssignVehicleRequested;
_vehicleAssignmentViewModel.RequestClose += OnAssignmentFormClosed;
}

    public async Task SetUser(string username, string role)
{
    Username = username;
    Role = role;

    await ShowDashboard();
}

    [RelayCommand]
private async Task ShowDashboard()
{
    var dashboardViewModel = new DashboardViewModel(_vehicleService);

    CurrentPage = dashboardViewModel;

    await dashboardViewModel.LoadAsync(Username, Role);
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
private async void OnEditVehicleRequested(
    object? sender,
    VehicleEditRequestedEventArgs e)
{
    await _vehicleFormViewModel
        .LoadForEditAsync(e.VehicleId);

    CurrentPage =
        _vehicleFormViewModel;
}
private void OnAddDriverRequested(object? sender, EventArgs e)
{
    CurrentPage = _driverFormViewModel;

    _ = LoadDriverFormAsync();
}

private async Task LoadDriverFormAsync()
{
    await _driverFormViewModel.LoadAsync();
}
[RelayCommand]
private async Task OpenDrivers()
{
    CurrentPage = _driversViewModel;

    await _driversViewModel.LoadAsync();
}
private void OnAssignVehicleRequested(
    object? sender,
    int driverId)
{
    CurrentPage = _vehicleAssignmentViewModel;

    _ = LoadAssignmentAsync(driverId);
}

private async Task LoadAssignmentAsync(int driverId)
{
    await _vehicleAssignmentViewModel.LoadAsync(driverId);
}

private async void OnAssignmentFormClosed(
    object? sender,
    EventArgs e)
{
    CurrentPage = _driversViewModel;

    await _driversViewModel.LoadAsync();
}
}