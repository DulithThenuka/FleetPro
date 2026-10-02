using CommunityToolkit.Mvvm.ComponentModel;
using FleetPro.Application.Interfaces;

namespace FleetPro.WPF.ViewModels;

public partial class DashboardViewModel
    : ObservableObject
{
    private readonly IVehicleService _vehicleService;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string role = string.Empty;

    [ObservableProperty]
    private int totalVehicles;

    [ObservableProperty]
    private int availableVehicles;

    [ObservableProperty]
    private int assignedVehicles;

    [ObservableProperty]
    private int vehiclesUnderMaintenance;

    [ObservableProperty]
    private int accidentVehicles;

    [ObservableProperty]
    private int retiredVehicles;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    public DashboardViewModel(
        IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async Task LoadAsync(
        string username,
        string role)
    {
        Username = username;
        Role = role;

        try
        {
            IsLoading = true;

            StatusMessage =
                "Loading dashboard...";

            var statistics =
                await _vehicleService
                    .GetStatisticsAsync();

            TotalVehicles =
                statistics.TotalVehicles;

            AvailableVehicles =
                statistics.AvailableVehicles;

            AssignedVehicles =
                statistics.AssignedVehicles;

            VehiclesUnderMaintenance =
                statistics.VehiclesUnderMaintenance;

            AccidentVehicles =
                statistics.AccidentVehicles;

            RetiredVehicles =
                statistics.RetiredVehicles;

            StatusMessage =
                "Dashboard updated successfully.";
        }
        catch
        {
            StatusMessage =
                "Unable to load dashboard statistics.";
        }
        finally
        {
            IsLoading = false;
        }
    }
}