using FleetPro.Application.DTOs;
using FleetPro.Domain.Entities;

namespace FleetPro.Application.Interfaces;

public interface IVehicleRepository
{
    Task<List<Vehicle>> GetAllAsync();

    Task<Vehicle?> GetByIdAsync(int vehicleId);

    Task<bool> RegistrationExistsAsync(
        string registrationNumber);

    Task<List<Branch>> GetBranchesAsync();

    Task<List<VehicleType>> GetVehicleTypesAsync();

    Task<VehicleStatisticsDto> GetStatisticsAsync();

    Task AddAsync(Vehicle vehicle);

    Task SaveChangesAsync();
}