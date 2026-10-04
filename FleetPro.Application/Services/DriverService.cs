using FleetPro.Application.DTOs;
using FleetPro.Application.Interfaces;
using FleetPro.Domain.Entities;
using FleetPro.Domain.Enums;

namespace FleetPro.Application.Services;

public class DriverService : IDriverService
{
    private readonly IDriverRepository _repository;

    public DriverService(
        IDriverRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DriverListItemDto>> GetAllAsync()
{
    var drivers = (await _repository.GetAllAsync()).ToList();

    var today = DateTime.Today;

    foreach (var driver in drivers)
    {
        if (!driver.LicenseExpiryDate.HasValue)
        {
            driver.LicenseStatus = "No expiry date";
            continue;
        }

        var daysRemaining =
            (driver.LicenseExpiryDate.Value.Date - today).Days;

        if (daysRemaining < 0)
        {
            driver.IsLicenseExpired = true;
            driver.LicenseStatus = "Expired";
        }
        else if (daysRemaining <= 30)
        {
            driver.IsLicenseExpiringSoon = true;
            driver.LicenseStatus =
                $"Expires in {daysRemaining} days";
        }
        else
        {
            driver.LicenseStatus = "Valid";
        }
    }

    return drivers;
}

    public async Task<(bool Success, string Message)>
        CreateAsync(
            CreateDriverRequest request)
    {
        var employeeNumber =
            request.EmployeeNumber.Trim();

        var licenseNumber =
            request.LicenseNumber.Trim();

        if (string.IsNullOrWhiteSpace(
                employeeNumber))
        {
            return (false,
                "Employee number is required.");
        }

        if (string.IsNullOrWhiteSpace(
                request.FullName))
        {
            return (false,
                "Driver name is required.");
        }

        if (string.IsNullOrWhiteSpace(
                licenseNumber))
        {
            return (false,
                "License number is required.");
        }

        if (request.BranchId <= 0)
        {
            return (false,
                "Please select a branch.");
        }

        if (request.LicenseExpiryDate.HasValue &&
            request.LicenseExpiryDate.Value.Date
            < DateTime.UtcNow.Date)
        {
            return (false,
                "License has already expired.");
        }

        if (await _repository
            .EmployeeNumberExistsAsync(
                employeeNumber))
        {
            return (false,
                "Employee number already exists.");
        }

        if (await _repository
            .LicenseNumberExistsAsync(
                licenseNumber))
        {
            return (false,
                "License number already exists.");
        }

        var driver = new Driver
        {
            EmployeeNumber =
                employeeNumber,

            FullName =
                request.FullName.Trim(),

            NIC =
                request.NIC?.Trim(),

            Phone =
                request.Phone?.Trim(),

            Address =
                request.Address?.Trim(),

            LicenseNumber =
                licenseNumber,

            LicenseCategory =
                request.LicenseCategory?.Trim(),

            LicenseExpiryDate =
                request.LicenseExpiryDate,

            JoiningDate =
                request.JoiningDate,

            BranchId =
                request.BranchId,

            Status =
                DriverStatus.Active
        };

        await _repository.AddAsync(driver);

        await _repository.SaveChangesAsync();

        return (true,
            "Driver registered successfully.");
    }

    public async Task<DriverListItemDto?> GetByIdAsync(int driverId)
{
    var driver = await _repository.GetByIdAsync(driverId);

    if (driver is null)
        return null;

    var activeAssignment = driver.VehicleAssignments
        .FirstOrDefault(x =>
            x.Status == AssignmentStatus.Active);

    return new DriverListItemDto
    {
        DriverId = driver.DriverId,

        EmployeeNumber = driver.EmployeeNumber,

        FullName = driver.FullName,

        LicenseNumber = driver.LicenseNumber,

        LicenseExpiryDate = driver.LicenseExpiryDate,

        Branch = driver.Branch?.BranchName ?? string.Empty,

        Status = driver.Status.ToString(),

        HasActiveAssignment = activeAssignment is not null,

        ActiveAssignmentId = activeAssignment?.AssignmentId,

        AssignedVehicle = activeAssignment?.Vehicle is not null
            ? $"{activeAssignment.Vehicle.RegistrationNumber} - " +
              $"{activeAssignment.Vehicle.Brand} " +
              $"{activeAssignment.Vehicle.Model}"
            : null
    };
}

    public async Task<(bool Success, string Message)>
        AssignVehicleAsync(
            AssignVehicleRequest request)
    {
        var driver =
            await _repository.GetByIdAsync(
                request.DriverId);

        if (driver == null)
        {
            return (false,
                "Driver could not be found.");
        }

        var vehicle =
            await _repository.GetVehicleByIdAsync(
                request.VehicleId);

        if (vehicle == null)
        {
            return (false,
                "Vehicle could not be found.");
        }

        if (driver.Status !=
            DriverStatus.Active)
        {
            return (false,
                "Only active drivers can be assigned.");
        }

        if (driver.LicenseExpiryDate.HasValue &&
            driver.LicenseExpiryDate.Value.Date
            < DateTime.UtcNow.Date)
        {
            return (false,
                "Driver license has expired.");
        }

        var driverAlreadyAssigned =
            driver.VehicleAssignments.Any(x =>
                x.Status ==
                AssignmentStatus.Active);

        if (driverAlreadyAssigned)
        {
            return (false,
                "Driver already has an active vehicle assignment.");
        }

        if (vehicle.Status !=
            VehicleStatus.Available)
        {
            return (false,
                "Vehicle is not available for assignment.");
        }

        if (request.StartMileage.HasValue &&
            request.StartMileage.Value
            < vehicle.CurrentMileage)
        {
            return (false,
                "Assignment mileage cannot be lower than the current vehicle mileage.");
        }

        var assignment = new VehicleAssignment
        {
            DriverId =
                driver.DriverId,

            VehicleId =
                vehicle.VehicleId,

            StartDate =
                request.StartDate,

            StartMileage =
                request.StartMileage
                ?? vehicle.CurrentMileage,

            Status =
                AssignmentStatus.Active,

            Notes =
                request.Notes?.Trim()
        };

        vehicle.Status =
            VehicleStatus.Assigned;

        await _repository.AddAssignmentAsync(
            assignment);

        await _repository.SaveChangesAsync();

        return (true,
            "Vehicle assigned successfully.");
    }

    public async Task<IEnumerable<LookupItemDto>> GetBranchesAsync()
{
    return await _repository.GetBranchesAsync();
}

public async Task CompleteAssignmentAsync(
    int assignmentId,
    DateTime endDate,
    decimal? endMileage,
    string? notes)
{
    var assignment =
        await _repository.GetAssignmentByIdAsync(assignmentId);

    if (assignment is null)
        throw new InvalidOperationException("Assignment not found.");

    if (assignment.Status != AssignmentStatus.Active)
        throw new InvalidOperationException(
            "Only active assignments can be completed.");

    if (endDate.Date < assignment.StartDate.Date)
        throw new InvalidOperationException(
            "End date cannot be before the start date.");

    if (endDate.Date > DateTime.Today)
        throw new InvalidOperationException(
            "End date cannot be in the future.");

    if (endMileage.HasValue)
    {
        if (endMileage.Value < 0)
            throw new InvalidOperationException(
                "End mileage cannot be negative.");

        if (assignment.StartMileage.HasValue &&
            endMileage.Value < assignment.StartMileage.Value)
        {
            throw new InvalidOperationException(
                "End mileage cannot be lower than start mileage.");
        }

        if (endMileage.Value < assignment.Vehicle.CurrentMileage)
        {
            throw new InvalidOperationException(
                "End mileage cannot be lower than the vehicle's current mileage.");
        }

        assignment.EndMileage = endMileage.Value;
        assignment.Vehicle.CurrentMileage = endMileage.Value;
    }

    assignment.EndDate = endDate;

    if (!string.IsNullOrWhiteSpace(notes))
    {
        assignment.Notes = string.IsNullOrWhiteSpace(assignment.Notes)
            ? notes.Trim()
            : $"{assignment.Notes}\n{notes.Trim()}";
    }

    assignment.Status = AssignmentStatus.Completed;

    assignment.Vehicle.Status = VehicleStatus.Available;

    await _repository.SaveChangesAsync();
}
public async Task UpdateAsync(UpdateDriverRequest request)
{
    var driver = await _repository.GetByIdAsync(request.DriverId);

    if (driver is null)
        throw new InvalidOperationException("Driver not found.");

    if (string.IsNullOrWhiteSpace(request.EmployeeNumber))
        throw new InvalidOperationException(
            "Employee number is required.");

    if (string.IsNullOrWhiteSpace(request.FullName))
        throw new InvalidOperationException(
            "Full name is required.");

    if (string.IsNullOrWhiteSpace(request.LicenseNumber))
        throw new InvalidOperationException(
            "License number is required.");

    if (request.BranchId <= 0)
        throw new InvalidOperationException(
            "Branch is required.");

    if (request.LicenseExpiryDate.HasValue &&
        request.LicenseExpiryDate.Value.Date < DateTime.Today)
    {
        throw new InvalidOperationException(
            "License has expired.");
    }

    if (!string.Equals(
            driver.EmployeeNumber,
            request.EmployeeNumber,
            StringComparison.OrdinalIgnoreCase))
    {
        if (await _repository.EmployeeNumberExistsAsync(
                request.EmployeeNumber))
        {
            throw new InvalidOperationException(
                "Employee number already exists.");
        }
    }

    if (!string.Equals(
            driver.LicenseNumber,
            request.LicenseNumber,
            StringComparison.OrdinalIgnoreCase))
    {
        if (await _repository.LicenseNumberExistsAsync(
                request.LicenseNumber))
        {
            throw new InvalidOperationException(
                "License number already exists.");
        }
    }

    driver.EmployeeNumber = request.EmployeeNumber.Trim();
    driver.FullName = request.FullName.Trim();
    driver.NIC = request.NIC?.Trim();
    driver.Phone = request.Phone?.Trim();
    driver.Address = request.Address?.Trim();
    driver.LicenseNumber = request.LicenseNumber.Trim();
    driver.LicenseCategory = request.LicenseCategory?.Trim();
    driver.LicenseExpiryDate = request.LicenseExpiryDate;
    driver.JoiningDate = request.JoiningDate;
    driver.BranchId = request.BranchId;
    driver.UpdatedAt = DateTime.UtcNow;

    await _repository.SaveChangesAsync();
}
}