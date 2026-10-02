using FleetPro.Domain.Enums;

namespace FleetPro.Domain.Entities;

public class Driver
{
    public int DriverId { get; set; }

    public string EmployeeNumber { get; set; }
        = string.Empty;

    public string FullName { get; set; }
        = string.Empty;

    public string? NIC { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string LicenseNumber { get; set; }
        = string.Empty;

    public string? LicenseCategory { get; set; }

    public DateTime? LicenseExpiryDate { get; set; }

    public DateTime? JoiningDate { get; set; }

    public int BranchId { get; set; }

    public DriverStatus Status { get; set; }
        = DriverStatus.Active;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public Branch Branch { get; set; } = null!;

    public ICollection<VehicleAssignment> VehicleAssignments { get; set; }
        = new List<VehicleAssignment>();
}