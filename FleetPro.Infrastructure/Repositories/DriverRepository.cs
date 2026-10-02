using FleetPro.Application.Interfaces;
using FleetPro.Domain.Entities;
using FleetPro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FleetPro.Application.DTOs;


namespace FleetPro.Infrastructure.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly FleetProDbContext _context;

    public DriverRepository(
        FleetProDbContext context)
    {
        _context = context;
    }

    public async Task<List<Driver>> GetAllAsync()
    {
        return await _context.Drivers
            .Include(x => x.Branch)
            .Include(x => x.VehicleAssignments)
                .ThenInclude(x => x.Vehicle)
            .OrderBy(x => x.FullName)
            .ToListAsync();
    }

    public async Task<Driver?> GetByIdAsync(
        int driverId)
    {
        return await _context.Drivers
            .Include(x => x.Branch)
            .Include(x => x.VehicleAssignments)
                .ThenInclude(x => x.Vehicle)
            .FirstOrDefaultAsync(
                x => x.DriverId == driverId);
    }

    public async Task<Vehicle?> GetVehicleByIdAsync(
        int vehicleId)
    {
        return await _context.Vehicles
            .FirstOrDefaultAsync(
                x => x.VehicleId == vehicleId);
    }

    public async Task<bool> EmployeeNumberExistsAsync(
        string employeeNumber)
    {
        return await _context.Drivers
            .AnyAsync(x =>
                x.EmployeeNumber.ToUpper() ==
                employeeNumber.ToUpper());
    }

    public async Task<bool> LicenseNumberExistsAsync(
        string licenseNumber)
    {
        return await _context.Drivers
            .AnyAsync(x =>
                x.LicenseNumber.ToUpper() ==
                licenseNumber.ToUpper());
    }

    public async Task AddAsync(
        Driver driver)
    {
        await _context.Drivers.AddAsync(driver);
    }

    public async Task AddAssignmentAsync(
        VehicleAssignment assignment)
    {
        await _context.VehicleAssignments
            .AddAsync(assignment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<LookupItemDto>> GetBranchesAsync()
{
    return await _context.Branches
        .Where(b => b.IsActive)
        .OrderBy(b => b.BranchName)
        .Select(b => new LookupItemDto
        {
            Id = b.BranchId,
            Name = $"{b.BranchCode} - {b.BranchName}"
        })
        .ToListAsync();
}
}