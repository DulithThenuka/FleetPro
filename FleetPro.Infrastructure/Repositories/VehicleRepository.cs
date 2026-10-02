using FleetPro.Application.Interfaces;
using FleetPro.Domain.Entities;
using FleetPro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FleetPro.Application.DTOs;

namespace FleetPro.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly FleetProDbContext _context;

    public VehicleRepository(
        FleetProDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles
            .Include(x => x.Branch)
            .Include(x => x.VehicleType)
            .OrderBy(x => x.RegistrationNumber)
            .ToListAsync();
    }

    public async Task<Vehicle?> GetByIdAsync(
        int vehicleId)
    {
        return await _context.Vehicles
            .Include(x => x.Branch)
            .Include(x => x.VehicleType)
            .FirstOrDefaultAsync(
                x => x.VehicleId == vehicleId);
    }

    public async Task<bool> RegistrationExistsAsync(
        string registrationNumber)
    {
        return await _context.Vehicles
            .AnyAsync(x =>
                x.RegistrationNumber.ToUpper() ==
                registrationNumber.ToUpper());
    }

    public async Task<List<Branch>> GetBranchesAsync()
    {
        return await _context.Branches
            .Where(x => x.IsActive)
            .OrderBy(x => x.BranchName)
            .ToListAsync();
    }

    public async Task<List<VehicleType>> GetVehicleTypesAsync()
    {
        return await _context.VehicleTypes
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<VehicleStatisticsDto> GetStatisticsAsync()
{
    return new VehicleStatisticsDto
    {
        TotalVehicles =
            await _context.Vehicles.CountAsync(),

        AvailableVehicles =
            await _context.Vehicles.CountAsync(
                x => x.Status ==
                     Domain.Enums.VehicleStatus.Available),

        AssignedVehicles =
            await _context.Vehicles.CountAsync(
                x => x.Status ==
                     Domain.Enums.VehicleStatus.Assigned),

        VehiclesUnderMaintenance =
            await _context.Vehicles.CountAsync(
                x => x.Status ==
                     Domain.Enums.VehicleStatus.UnderMaintenance),

        AccidentVehicles =
            await _context.Vehicles.CountAsync(
                x => x.Status ==
                     Domain.Enums.VehicleStatus.Accident),

        InactiveVehicles =
            await _context.Vehicles.CountAsync(
                x => x.Status ==
                     Domain.Enums.VehicleStatus.Inactive),

        RetiredVehicles =
            await _context.Vehicles.CountAsync(
                x => x.Status ==
                     Domain.Enums.VehicleStatus.Retired),

        SoldVehicles =
            await _context.Vehicles.CountAsync(
                x => x.Status ==
                     Domain.Enums.VehicleStatus.Sold)
    };
}
}