namespace FleetPro.Application.DTOs;

public class DriverListItemDto
{
    public int DriverId { get; set; }

    public string EmployeeNumber { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public DateTime? LicenseExpiryDate { get; set; }

    public string Branch { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public bool HasActiveAssignment { get; set; }

    public int? ActiveAssignmentId { get; set; }

    public string? AssignedVehicle { get; set; }

    public bool IsLicenseExpired { get; set; }

public bool IsLicenseExpiringSoon { get; set; }

public string LicenseStatus { get; set; } = string.Empty;
}