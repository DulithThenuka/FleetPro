using FleetPro.Application.DTOs;
using FleetPro.Domain.Entities;

namespace FleetPro.Application.Interfaces;

public interface IDriverRepository
{
    Task<IEnumerable<DriverListItemDto>> GetAllAsync();

    Task<Driver?> GetByIdAsync(
        int driverId);

    Task<Vehicle?> GetVehicleByIdAsync(
        int vehicleId);

    Task<VehicleAssignment?> GetAssignmentByIdAsync(
        int assignmentId);

    Task<IEnumerable<LookupItemDto>> GetBranchesAsync();

    Task<bool> EmployeeNumberExistsAsync(
        string employeeNumber);

    Task<bool> LicenseNumberExistsAsync(
        string licenseNumber);

    Task AddAsync(
        Driver driver);

    Task AddAssignmentAsync(
        VehicleAssignment assignment);

    Task SaveChangesAsync();
}