using FleetPro.Domain.Enums;

namespace FleetPro.Domain.Entities;

public class VehicleAssignment
{
    public int AssignmentId { get; set; }

    public int VehicleId { get; set; }

    public int DriverId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? StartMileage { get; set; }

    public decimal? EndMileage { get; set; }

    public AssignmentStatus Status { get; set; }
        = AssignmentStatus.Active;

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public Vehicle Vehicle { get; set; } = null!;

    public Driver Driver { get; set; } = null!;
}