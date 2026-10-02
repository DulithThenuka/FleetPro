using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FleetPro.Application.DTOs;
using FleetPro.Application.Interfaces;
using System.Collections.ObjectModel;

namespace FleetPro.WPF.ViewModels;

public partial class DriversViewModel : ObservableObject
{
    private readonly IDriverService _driverService;

    private List<DriverListItemDto> _allDrivers = new();

    [ObservableProperty]
    private ObservableCollection<DriverListItemDto> drivers = new();

    [ObservableProperty]
    private DriverListItemDto? selectedDriver;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    [ObservableProperty]
    private bool hasSelectedDriver;

    public event EventHandler? AddDriverRequested;
    public event EventHandler<int>? AssignVehicleRequested;

    public DriversViewModel(IDriverService driverService)
    {
        _driverService = driverService;
    }

    partial void OnSelectedDriverChanged(DriverListItemDto? value)
    {
        HasSelectedDriver = value is not null;
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilter();
    }

    public async Task LoadAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = string.Empty;

            var result = await _driverService.GetAllAsync();

            _allDrivers = result.ToList();

            ApplyFilter();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to load drivers: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ApplyFilter()
    {
        var query = SearchText.Trim();

        IEnumerable<DriverListItemDto> filtered = _allDrivers;

        if (!string.IsNullOrWhiteSpace(query))
        {
            filtered = _allDrivers.Where(d =>
                d.EmployeeNumber.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                d.FullName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                d.LicenseNumber.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                (d.Branch?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                d.Status.Contains(query, StringComparison.OrdinalIgnoreCase));
        }

        Drivers = new ObservableCollection<DriverListItemDto>(filtered);
    }

    [RelayCommand]
    private void AddDriver()
    {
        AddDriverRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void AssignVehicle()
    {
        if (SelectedDriver is null)
            return;

        AssignVehicleRequested?.Invoke(this, SelectedDriver.DriverId);
    }

    [RelayCommand]
    private async Task Refresh()
    {
        await LoadAsync();
    }
}