namespace FleetPro.Application.DTOs;

public class CreateDriverRequest
{
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
}