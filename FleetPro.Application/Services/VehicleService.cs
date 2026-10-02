using FleetPro.Application.DTOs;
using FleetPro.Application.Interfaces;
using FleetPro.Domain.Entities;
using FleetPro.Domain.Enums;

namespace FleetPro.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _repository;

    public VehicleService(
        IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<VehicleListItemDto>> GetAllAsync()
{
    var vehicles =
        await _repository.GetAllAsync();

    return vehicles.Select(x =>
        new VehicleListItemDto
        {
            VehicleId =
                x.VehicleId,

            RegistrationNumber =
                x.RegistrationNumber,

            VIN =
                x.VIN,

            EngineNumber =
                x.EngineNumber,

            Brand =
                x.Brand,

            Model =
                x.Model,

            ManufacturingYear =
                x.ManufacturingYear,

            VehicleTypeId =
                x.VehicleTypeId,

            VehicleType =
                x.VehicleType.Name,

            BranchId =
                x.BranchId,

            Branch =
                x.Branch.BranchName,

            FuelType =
                x.FuelType,

            Transmission =
                x.Transmission,

            Color =
                x.Color,

            PurchaseDate =
                x.PurchaseDate,

            PurchasePrice =
                x.PurchasePrice,

            CurrentMileage =
                x.CurrentMileage,

            Status =
                x.Status.ToString()

        }).ToList();
}

    public async Task<List<LookupItemDto>>
        GetBranchesAsync()
    {
        var branches =
            await _repository.GetBranchesAsync();

        return branches.Select(x =>
            new LookupItemDto
            {
                Id = x.BranchId,
                Name = x.BranchName
            }).ToList();
    }

    public async Task<List<LookupItemDto>>
        GetVehicleTypesAsync()
    {
        var types =
            await _repository.GetVehicleTypesAsync();

        return types.Select(x =>
            new LookupItemDto
            {
                Id = x.VehicleTypeId,
                Name = x.Name
            }).ToList();
    }

    public async Task<(bool Success, string Message)>
        CreateAsync(CreateVehicleRequest request)
    {
        var registration =
            request.RegistrationNumber.Trim();

        if (string.IsNullOrWhiteSpace(registration))
        {
            return (false,
                "Registration number is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Brand))
        {
            return (false,
                "Brand is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Model))
        {
            return (false,
                "Model is required.");
        }

        if (request.VehicleTypeId <= 0)
        {
            return (false,
                "Please select a vehicle type.");
        }

        if (request.BranchId <= 0)
        {
            return (false,
                "Please select a branch.");
        }

        if (request.ManufacturingYear < 1900 ||
            request.ManufacturingYear >
            DateTime.UtcNow.Year + 1)
        {
            return (false,
                "Please enter a valid manufacturing year.");
        }

        if (request.CurrentMileage < 0)
        {
            return (false,
                "Mileage cannot be negative.");
        }

        if (request.PurchasePrice is < 0)
        {
            return (false,
                "Purchase price cannot be negative.");
        }

        if (await _repository
            .RegistrationExistsAsync(registration))
        {
            return (false,
                "A vehicle with this registration already exists.");
        }

        var vehicle = new Vehicle
        {
            RegistrationNumber = registration,
            VIN = string.IsNullOrWhiteSpace(request.VIN)
                ? null
                : request.VIN.Trim(),

            EngineNumber =
                string.IsNullOrWhiteSpace(request.EngineNumber)
                    ? null
                    : request.EngineNumber.Trim(),

            VehicleTypeId =
                request.VehicleTypeId,

            BranchId =
                request.BranchId,

            Brand = request.Brand.Trim(),

            Model = request.Model.Trim(),

            ManufacturingYear =
                request.ManufacturingYear,

            FuelType =
                request.FuelType,

            Transmission =
                request.Transmission,

            Color =
                string.IsNullOrWhiteSpace(request.Color)
                    ? null
                    : request.Color.Trim(),

            PurchaseDate =
                request.PurchaseDate,

            PurchasePrice =
                request.PurchasePrice,

            CurrentMileage =
                request.CurrentMileage,

            Status =
                VehicleStatus.Available
        };

        await _repository.AddAsync(vehicle);

        await _repository.SaveChangesAsync();

        return (true,
            "Vehicle registered successfully.");
    }

    public async Task<VehicleDetailsDto?> GetByIdAsync(
    int vehicleId)
{
    var vehicle =
        await _repository.GetByIdAsync(vehicleId);

    if (vehicle == null)
    {
        return null;
    }

    return new VehicleDetailsDto
    {
        VehicleId = vehicle.VehicleId,

        RegistrationNumber =
            vehicle.RegistrationNumber,

        VIN = vehicle.VIN,

        EngineNumber =
            vehicle.EngineNumber,

        VehicleTypeId =
            vehicle.VehicleTypeId,

        BranchId =
            vehicle.BranchId,

        Brand =
            vehicle.Brand,

        Model =
            vehicle.Model,

        ManufacturingYear =
            vehicle.ManufacturingYear,

        FuelType =
            vehicle.FuelType,

        Transmission =
            vehicle.Transmission,

        Color =
            vehicle.Color,

        PurchaseDate =
            vehicle.PurchaseDate,

        PurchasePrice =
            vehicle.PurchasePrice,

        CurrentMileage =
            vehicle.CurrentMileage,

        Status =
            vehicle.Status
    };
}
public async Task<(bool Success, string Message)>
    UpdateAsync(UpdateVehicleRequest request)
{
    var vehicle =
        await _repository.GetByIdAsync(
            request.VehicleId);

    if (vehicle == null)
    {
        return (false,
            "Vehicle could not be found.");
    }

    var registration =
        request.RegistrationNumber.Trim();

    if (string.IsNullOrWhiteSpace(registration))
    {
        return (false,
            "Registration number is required.");
    }

    if (string.IsNullOrWhiteSpace(request.Brand))
    {
        return (false,
            "Brand is required.");
    }

    if (string.IsNullOrWhiteSpace(request.Model))
    {
        return (false,
            "Model is required.");
    }

    if (request.VehicleTypeId <= 0)
    {
        return (false,
            "Please select a vehicle type.");
    }

    if (request.BranchId <= 0)
    {
        return (false,
            "Please select a branch.");
    }

    if (request.ManufacturingYear < 1900 ||
        request.ManufacturingYear >
        DateTime.UtcNow.Year + 1)
    {
        return (false,
            "Please enter a valid manufacturing year.");
    }

    if (request.CurrentMileage < 0)
    {
        return (false,
            "Mileage cannot be negative.");
    }

    if (request.PurchasePrice is < 0)
    {
        return (false,
            "Purchase price cannot be negative.");
    }

    var duplicateRegistration =
        await _repository
            .RegistrationExistsAsync(registration);

    if (duplicateRegistration &&
        !string.Equals(
            vehicle.RegistrationNumber,
            registration,
            StringComparison.OrdinalIgnoreCase))
    {
        return (false,
            "Another vehicle already uses this registration.");
    }

    vehicle.RegistrationNumber = registration;

    vehicle.VIN =
        string.IsNullOrWhiteSpace(request.VIN)
            ? null
            : request.VIN.Trim();

    vehicle.EngineNumber =
        string.IsNullOrWhiteSpace(request.EngineNumber)
            ? null
            : request.EngineNumber.Trim();

    vehicle.VehicleTypeId =
        request.VehicleTypeId;

    vehicle.BranchId =
        request.BranchId;

    vehicle.Brand =
        request.Brand.Trim();

    vehicle.Model =
        request.Model.Trim();

    vehicle.ManufacturingYear =
        request.ManufacturingYear;

    vehicle.FuelType =
        request.FuelType;

    vehicle.Transmission =
        request.Transmission;

    vehicle.Color =
        string.IsNullOrWhiteSpace(request.Color)
            ? null
            : request.Color.Trim();

    vehicle.PurchaseDate =
        request.PurchaseDate;

    vehicle.PurchasePrice =
        request.PurchasePrice;

    vehicle.CurrentMileage =
        request.CurrentMileage;

    vehicle.UpdatedAt =
        DateTime.UtcNow;

    await _repository.SaveChangesAsync();

    return (true,
        "Vehicle updated successfully.");
}
public async Task<VehicleStatisticsDto>
    GetStatisticsAsync()
{
    return await _repository.GetStatisticsAsync();
}
public async Task<(bool Success, string Message)>
    RetireAsync(int vehicleId)
{
    var vehicle =
        await _repository.GetByIdAsync(vehicleId);

    if (vehicle == null)
    {
        return (false,
            "Vehicle could not be found.");
    }

    if (vehicle.Status ==
        VehicleStatus.Retired)
    {
        return (false,
            "Vehicle is already retired.");
    }

    if (vehicle.Status ==
        VehicleStatus.Sold)
    {
        return (false,
            "A sold vehicle cannot be retired.");
    }

    if (vehicle.Status ==
        VehicleStatus.Assigned)
    {
        return (false,
            "An assigned vehicle cannot be retired. Remove the assignment first.");
    }

    if (vehicle.Status ==
        VehicleStatus.UnderMaintenance)
    {
        return (false,
            "A vehicle under maintenance cannot be retired.");
    }

    vehicle.Status =
        VehicleStatus.Retired;

    vehicle.UpdatedAt =
        DateTime.UtcNow;

    await _repository.SaveChangesAsync();

    return (true,
        "Vehicle retired successfully.");
}
}