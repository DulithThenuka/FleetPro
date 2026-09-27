namespace FleetPro.Application.DTOs;

public class VehicleListItemDto
{
    public int VehicleId { get; set; }

    public string RegistrationNumber { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string VehicleType { get; set; } = string.Empty;

    public string Branch { get; set; } = string.Empty;

    public string FuelType { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public decimal CurrentMileage { get; set; }
}