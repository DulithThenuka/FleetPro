using FleetPro.Domain.Enums;

namespace FleetPro.Application.DTOs;

public class CreateVehicleRequest
{
    public string RegistrationNumber { get; set; } = string.Empty;

    public string? VIN { get; set; }

    public string? EngineNumber { get; set; }

    public int VehicleTypeId { get; set; }

    public int BranchId { get; set; }

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int ManufacturingYear { get; set; }

    public FuelType FuelType { get; set; }

    public TransmissionType Transmission { get; set; }

    public string? Color { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public decimal? PurchasePrice { get; set; }

    public decimal CurrentMileage { get; set; }
}