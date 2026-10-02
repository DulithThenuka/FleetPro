namespace FleetPro.Application.DTOs;

public class AssignVehicleRequest
{
    public int DriverId { get; set; }

    public int VehicleId { get; set; }

    public DateTime StartDate { get; set; }

    public decimal? StartMileage { get; set; }

    public string? Notes { get; set; }
}