using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FleetPro.Application.DTOs;
using FleetPro.Application.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FleetPro.WPF.ViewModels;

public partial class VehicleAssignmentViewModel : ObservableObject
{
    private readonly IDriverService _driverService;
    private readonly IVehicleService _vehicleService;

    private int _driverId;

    [ObservableProperty]
    private string driverName = string.Empty;

    [ObservableProperty]
    private string employeeNumber = string.Empty;

    [ObservableProperty]
    private ObservableCollection<VehicleListItemDto> availableVehicles = new();

    [ObservableProperty]
    private VehicleListItemDto? selectedVehicle;

    [ObservableProperty]
    private DateTime startDate = DateTime.Today;

    [ObservableProperty]
    private string startMileage = string.Empty;

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private string formMessage = string.Empty;

    [ObservableProperty]
    private bool isSaving;

    public event EventHandler? RequestClose;

    public VehicleAssignmentViewModel(
        IDriverService driverService,
        IVehicleService vehicleService)
    {
        _driverService = driverService;
        _vehicleService = vehicleService;
    }

    public async Task LoadAsync(int driverId)
    {
        _driverId = driverId;

        FormMessage = string.Empty;
        StartDate = DateTime.Today;
        StartMileage = string.Empty;
        Notes = string.Empty;

        var driver = await _driverService.GetByIdAsync(driverId);

        if (driver is null)
        {
            FormMessage = "Driver not found.";
            return;
        }

        DriverName = driver.FullName;
        EmployeeNumber = driver.EmployeeNumber;

        if (driver.Status != "Active")
        {
            FormMessage = "Only active drivers can be assigned a vehicle.";
            return;
        }

        if (driver.LicenseExpiryDate.HasValue &&
            driver.LicenseExpiryDate.Value.Date < DateTime.Today)
        {
            FormMessage = "This driver's license has expired.";
            return;
        }

        if (driver.HasActiveAssignment)
        {
            FormMessage = "This driver already has an active vehicle assignment.";
            return;
        }

        var vehicles = await _vehicleService.GetAllAsync();

        var available = vehicles
            .Where(v => v.Status == "Available")
            .OrderBy(v => v.RegistrationNumber)
            .ToList();

        AvailableVehicles =
            new ObservableCollection<VehicleListItemDto>(available);

        if (AvailableVehicles.Count > 0)
            SelectedVehicle = AvailableVehicles[0];
        else
            FormMessage = "There are no available vehicles.";
    }

    partial void OnSelectedVehicleChanged(VehicleListItemDto? value)
    {
        if (value is not null)
        {
            StartMileage = value.CurrentMileage.ToString("0.0");
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        if (IsSaving)
            return;

        FormMessage = string.Empty;

        if (SelectedVehicle is null)
        {
            FormMessage = "Please select a vehicle.";
            return;
        }

        if (StartDate.Date < DateTime.Today)
        {
            FormMessage = "Start date cannot be in the past.";
            return;
        }

        decimal? mileage = null;

        if (!string.IsNullOrWhiteSpace(StartMileage))
        {
            if (!decimal.TryParse(StartMileage, out var parsedMileage))
            {
                FormMessage = "Enter a valid start mileage.";
                return;
            }

            if (parsedMileage < SelectedVehicle.CurrentMileage)
            {
                FormMessage =
                    $"Start mileage cannot be below {SelectedVehicle.CurrentMileage:0.0}.";
                return;
            }

            mileage = parsedMileage;
        }

        try
        {
            IsSaving = true;

            var request = new AssignVehicleRequest
            {
                DriverId = _driverId,
                VehicleId = SelectedVehicle.VehicleId,
                StartDate = StartDate,
                StartMileage = mileage,
                Notes = string.IsNullOrWhiteSpace(Notes)
                    ? null
                    : Notes.Trim()
            };

            await _driverService.AssignVehicleAsync(request);

            FormMessage = "Vehicle assigned successfully.";

            RequestClose?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            FormMessage = ex.Message;
        }
        finally
        {
            IsSaving = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}