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
                VehicleId = x.VehicleId,
                RegistrationNumber = x.RegistrationNumber,
                Brand = x.Brand,
                Model = x.Model,
                VehicleType = x.VehicleType.Name,
                Branch = x.Branch.BranchName,
                FuelType = x.FuelType.ToString(),
                Status = x.Status.ToString(),
                CurrentMileage = x.CurrentMileage
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
            return (false, "Brand is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Model))
        {
            return (false, "Model is required.");
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

        if (await _repository
            .RegistrationExistsAsync(registration))
        {
            return (false,
                "A vehicle with this registration already exists.");
        }

        var vehicle = new Vehicle
        {
            RegistrationNumber = registration,
            VIN = request.VIN,
            EngineNumber = request.EngineNumber,
            VehicleTypeId = request.VehicleTypeId,
            BranchId = request.BranchId,
            Brand = request.Brand.Trim(),
            Model = request.Model.Trim(),
            ManufacturingYear =
                request.ManufacturingYear,
            FuelType = request.FuelType,
            Transmission = request.Transmission,
            Color = request.Color,
            PurchaseDate = request.PurchaseDate,
            PurchasePrice = request.PurchasePrice,
            CurrentMileage = request.CurrentMileage,
            Status = VehicleStatus.Available
        };

        await _repository.AddAsync(vehicle);

        await _repository.SaveChangesAsync();

        return (true,
            "Vehicle registered successfully.");
    }
}