using FleetPro.Domain.Entities;
using FleetPro.Application.DTOs;

namespace FleetPro.Application.Interfaces;

public interface IDriverRepository
{
    Task<List<Driver>> GetAllAsync();

    Task<Driver?> GetByIdAsync(
        int driverId);

    Task<Vehicle?> GetVehicleByIdAsync(
        int vehicleId);

    Task<bool> EmployeeNumberExistsAsync(
        string employeeNumber);

    Task<bool> LicenseNumberExistsAsync(
        string licenseNumber);

    Task AddAsync(Driver driver);

    Task AddAssignmentAsync(
        VehicleAssignment assignment);

    Task SaveChangesAsync();

    Task<IEnumerable<LookupItemDto>> GetBranchesAsync();

    
}