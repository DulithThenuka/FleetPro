using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FleetPro.Application.DTOs;
using FleetPro.Application.Interfaces;

namespace FleetPro.WPF.ViewModels;

public partial class VehiclesViewModel : ObservableObject
{
    private readonly IVehicleService _vehicleService;

    public ObservableCollection<VehicleListItemDto> Vehicles { get; }
        = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string statusMessage = string.Empty;

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
            StatusMessage = "Loading vehicles...";

            var vehicles = await _vehicleService.GetAllAsync();

            Vehicles.Clear();

            foreach (var vehicle in vehicles)
            {
                Vehicles.Add(vehicle);
            }

            StatusMessage =
                $"{Vehicles.Count} vehicle(s) found.";
        }
        catch (Exception)
        {
            StatusMessage =
                "Unable to load vehicles.";
        }
        finally
        {
            IsLoading = false;
        }
    }
}