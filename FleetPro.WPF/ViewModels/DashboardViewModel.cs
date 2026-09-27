using CommunityToolkit.Mvvm.ComponentModel;

namespace FleetPro.WPF.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string role = string.Empty;

    [ObservableProperty]
    private int totalVehicles;

    [ObservableProperty]
    private int activeVehicles;

    [ObservableProperty]
    private int vehiclesUnderMaintenance;

    [ObservableProperty]
    private int pendingMaintenance;

    public DashboardViewModel(
        string username,
        string role)
    {
        Username = username;
        Role = role;

        // Temporary values.
        // These will come from SQL Server later.
        TotalVehicles = 0;
        ActiveVehicles = 0;
        VehiclesUnderMaintenance = 0;
        PendingMaintenance = 0;
    }
}