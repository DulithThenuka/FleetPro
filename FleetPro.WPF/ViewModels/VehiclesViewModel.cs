using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FleetPro.Application.DTOs;
using FleetPro.Application.Interfaces;

namespace FleetPro.WPF.ViewModels;

public partial class VehiclesViewModel : ObservableObject
{
    private readonly IVehicleService _vehicleService;

    private List<VehicleListItemDto> _allVehicles = new();

    public ObservableCollection<VehicleListItemDto> Vehicles { get; }
        = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    [ObservableProperty]
    private VehicleListItemDto? selectedVehicle;

    public bool HasSelectedVehicle =>
        SelectedVehicle != null;

    public event EventHandler? AddVehicleRequested;

    public VehiclesViewModel(
        IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async Task LoadAsync()
    {
        try
        {
            IsLoading = true;

            StatusMessage =
                "Loading vehicles...";

            _allVehicles =
                await _vehicleService.GetAllAsync();

            FilterVehicles();
        }
        catch
        {
            StatusMessage =
                "Unable to load vehicles.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    partial void OnSearchTextChanged(
        string value)
    {
        FilterVehicles();
    }

    partial void OnSelectedVehicleChanged(
        VehicleListItemDto? value)
    {
        OnPropertyChanged(
            nameof(HasSelectedVehicle));
    }

    private void FilterVehicles()
    {
        var query =
            SearchText.Trim();

        IEnumerable<VehicleListItemDto> filtered =
            _allVehicles;

        if (!string.IsNullOrWhiteSpace(query))
        {
            filtered =
                _allVehicles.Where(vehicle =>
                    vehicle.RegistrationNumber
                        .Contains(
                            query,
                            StringComparison.OrdinalIgnoreCase)

                    || vehicle.Brand
                        .Contains(
                            query,
                            StringComparison.OrdinalIgnoreCase)

                    || vehicle.Model
                        .Contains(
                            query,
                            StringComparison.OrdinalIgnoreCase)

                    || vehicle.VehicleType
                        .Contains(
                            query,
                            StringComparison.OrdinalIgnoreCase)

                    || vehicle.Branch
                        .Contains(
                            query,
                            StringComparison.OrdinalIgnoreCase)

                    || vehicle.Status
                        .Contains(
                            query,
                            StringComparison.OrdinalIgnoreCase));
        }

        Vehicles.Clear();

        foreach (var vehicle in filtered)
        {
            Vehicles.Add(vehicle);
        }

        StatusMessage =
            $"{Vehicles.Count} vehicle(s) found.";
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private void AddVehicle()
    {
        AddVehicleRequested?.Invoke(
            this,
            EventArgs.Empty);
    }

    public event EventHandler<VehicleEditRequestedEventArgs>?
    EditVehicleRequested;
    [CommunityToolkit.Mvvm.Input.RelayCommand]
private void EditVehicle(
    VehicleListItemDto vehicle)
{
    EditVehicleRequested?.Invoke(
        this,
        new VehicleEditRequestedEventArgs(
            vehicle.VehicleId));
}

}