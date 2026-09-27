using FleetPro.Application.Interfaces;
using FleetPro.Domain.Entities;
using FleetPro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
                x.RegistrationNumber
                    .ToUpper() ==
                registrationNumber
                    .ToUpper());
    }

    public async Task AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}