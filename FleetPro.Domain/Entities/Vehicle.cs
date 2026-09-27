using FleetPro.Domain.Enums;

namespace FleetPro.Domain.Entities;

public class Vehicle
{
    public int VehicleId { get; set; }

    public string RegistrationNumber { get; set; }
        = string.Empty;

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

    public VehicleStatus Status { get; set; }
        = VehicleStatus.Available;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties

    public Branch Branch { get; set; } = null!;

    public VehicleType VehicleType { get; set; } = null!;
}