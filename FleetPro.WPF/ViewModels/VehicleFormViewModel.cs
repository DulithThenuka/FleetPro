using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FleetPro.Application.DTOs;
using FleetPro.Application.Interfaces;
using FleetPro.Domain.Enums;

namespace FleetPro.WPF.ViewModels;

public partial class VehicleFormViewModel : ObservableObject
{
    private readonly IVehicleService _vehicleService;

    public ObservableCollection<LookupItemDto> Branches { get; }
        = new();

    public ObservableCollection<LookupItemDto> VehicleTypes { get; }
        = new();

    public Array FuelTypes =>
        Enum.GetValues(typeof(FuelType));

    public Array TransmissionTypes =>
        Enum.GetValues(typeof(TransmissionType));

    [ObservableProperty]
    private string registrationNumber = string.Empty;

    [ObservableProperty]
    private string vin = string.Empty;

    [ObservableProperty]
    private string engineNumber = string.Empty;

    [ObservableProperty]
    private int selectedVehicleTypeId;

    [ObservableProperty]
    private int selectedBranchId;

    [ObservableProperty]
    private string brand = string.Empty;

    [ObservableProperty]
    private string model = string.Empty;

    [ObservableProperty]
    private string manufacturingYear = string.Empty;

    [ObservableProperty]
    private FuelType selectedFuelType =
        FuelType.Petrol;

    [ObservableProperty]
    private TransmissionType selectedTransmission =
        TransmissionType.Manual;

    [ObservableProperty]
    private string color = string.Empty;

    [ObservableProperty]
    private DateTime? purchaseDate;

    [ObservableProperty]
    private string purchasePrice = string.Empty;

    [ObservableProperty]
    private string currentMileage = "0";

    [ObservableProperty]
    private string formMessage = string.Empty;

    [ObservableProperty]
private int vehicleId;

[ObservableProperty]
private bool isEditMode;

public string FormTitle =>
    IsEditMode
        ? "Edit Vehicle"
        : "Add Vehicle";

public string SaveButtonText =>
    IsEditMode
        ? "Update Vehicle"
        : "Save Vehicle";

    [ObservableProperty]
    private bool isSaving;

    public event EventHandler? RequestClose;

    public VehicleFormViewModel(
        IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async Task LoadAsync()
{
    // Reset form state
    VehicleId = 0;
    IsEditMode = false;

    RegistrationNumber = string.Empty;
    Vin = string.Empty;
    EngineNumber = string.Empty;

    Brand = string.Empty;
    Model = string.Empty;

    ManufacturingYear = string.Empty;
    Color = string.Empty;

    PurchaseDate = null;
    PurchasePrice = string.Empty;
    CurrentMileage = "0";

    SelectedFuelType = FuelType.Petrol;
    SelectedTransmission = TransmissionType.Manual;

    FormMessage = string.Empty;


    Branches.Clear();
    VehicleTypes.Clear();

    var branches =
        await _vehicleService.GetBranchesAsync();

    var vehicleTypes =
        await _vehicleService.GetVehicleTypesAsync();

    foreach (var branch in branches)
    {
        Branches.Add(branch);
    }

    foreach (var type in vehicleTypes)
    {
        VehicleTypes.Add(type);
    }

    if (Branches.Count > 0)
    {
        SelectedBranchId = Branches[0].Id;
    }

    if (VehicleTypes.Count > 0)
    {
        SelectedVehicleTypeId =
            VehicleTypes[0].Id;
    }
}

    [RelayCommand]
private async Task Save()
{
    FormMessage = string.Empty;

    if (!int.TryParse(
            ManufacturingYear,
            out var year))
    {
        FormMessage =
            "Manufacturing year must be a valid number.";

        return;
    }

    if (!decimal.TryParse(
            CurrentMileage,
            out var mileage))
    {
        FormMessage =
            "Mileage must be a valid number.";

        return;
    }

    decimal purchasePrice = 0;

    if (!string.IsNullOrWhiteSpace(PurchasePrice) &&
        !decimal.TryParse(
            PurchasePrice,
            out purchasePrice))
    {
        FormMessage =
            "Purchase price must be a valid number.";

        return;
    }

    try
    {
        IsSaving = true;

        if (IsEditMode)
        {
            var request = new UpdateVehicleRequest
            {
                VehicleId =
                    VehicleId,

                RegistrationNumber =
                    RegistrationNumber,

                VIN = Vin,

                EngineNumber =
                    EngineNumber,

                VehicleTypeId =
                    SelectedVehicleTypeId,

                BranchId =
                    SelectedBranchId,

                Brand =
                    Brand,

                Model =
                    Model,

                ManufacturingYear =
                    year,

                FuelType =
                    SelectedFuelType,

                Transmission =
                    SelectedTransmission,

                Color =
                    Color,

                PurchaseDate =
                    PurchaseDate,

                PurchasePrice =
                    purchasePrice,

                CurrentMileage =
                    mileage
            };

            var result =
                await _vehicleService
                    .UpdateAsync(request);

            FormMessage =
                result.Message;

            if (result.Success)
            {
                RequestClose?.Invoke(
                    this,
                    EventArgs.Empty);
            }
        }
        else
        {
            var request = new CreateVehicleRequest
            {
                RegistrationNumber =
                    RegistrationNumber,

                VIN =
                    Vin,

                EngineNumber =
                    EngineNumber,

                VehicleTypeId =
                    SelectedVehicleTypeId,

                BranchId =
                    SelectedBranchId,

                Brand =
                    Brand,

                Model =
                    Model,

                ManufacturingYear =
                    year,

                FuelType =
                    SelectedFuelType,

                Transmission =
                    SelectedTransmission,

                Color =
                    Color,

                PurchaseDate =
                    PurchaseDate,

                PurchasePrice =
                    purchasePrice,

                CurrentMileage =
                    mileage
            };

            var result =
                await _vehicleService
                    .CreateAsync(request);

            FormMessage =
                result.Message;

            if (result.Success)
            {
                RequestClose?.Invoke(
                    this,
                    EventArgs.Empty);
            }
        }
    }
    catch
    {
        FormMessage =
            IsEditMode
                ? "Unable to update the vehicle."
                : "Unable to save the vehicle.";
    }
    finally
    {
        IsSaving = false;
    }
}

    [RelayCommand]
    private void Cancel()
    {
        RequestClose?.Invoke(
            this,
            EventArgs.Empty);
    }

    partial void OnIsEditModeChanged(bool value)
{
    OnPropertyChanged(nameof(FormTitle));
    OnPropertyChanged(nameof(SaveButtonText));
}
public async Task LoadForEditAsync(
    int vehicleId)
{
    await LoadAsync();

    var vehicle =
        await _vehicleService
            .GetByIdAsync(vehicleId);

    if (vehicle == null)
    {
        FormMessage =
            "Vehicle could not be found.";

        return;
    }

    VehicleId =
        vehicle.VehicleId;

    IsEditMode = true;

    RegistrationNumber =
        vehicle.RegistrationNumber;

    Vin =
        vehicle.VIN ?? string.Empty;

    EngineNumber =
        vehicle.EngineNumber ?? string.Empty;

    SelectedVehicleTypeId =
        vehicle.VehicleTypeId;

    SelectedBranchId =
        vehicle.BranchId;

    Brand =
        vehicle.Brand;

    Model =
        vehicle.Model;

    ManufacturingYear =
        vehicle.ManufacturingYear.ToString();

    SelectedFuelType =
        vehicle.FuelType;

    SelectedTransmission =
        vehicle.Transmission;

    Color =
        vehicle.Color ?? string.Empty;

    PurchaseDate =
        vehicle.PurchaseDate;

    PurchasePrice =
        vehicle.PurchasePrice?.ToString("0.##")
        ?? "0";

    CurrentMileage =
        vehicle.CurrentMileage.ToString("0.##");

    FormMessage = string.Empty;
}
}