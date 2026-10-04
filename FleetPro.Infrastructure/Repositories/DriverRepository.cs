using FleetPro.Application.DTOs;
using FleetPro.Application.Interfaces;
using FleetPro.Domain.Entities;
using FleetPro.Domain.Enums;
using FleetPro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetPro.Infrastructure.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly FleetProDbContext _context;

    public DriverRepository(FleetProDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DriverListItemDto>> GetAllAsync()
    {
        return await _context.Drivers
            .Include(d => d.Branch)
            .Include(d => d.VehicleAssignments)
                .ThenInclude(a => a.Vehicle)
            .OrderBy(d => d.FullName)
            .Select(d => new DriverListItemDto
            {
                DriverId = d.DriverId,

                EmployeeNumber = d.EmployeeNumber,

                FullName = d.FullName,

                LicenseNumber = d.LicenseNumber,

                LicenseExpiryDate = d.LicenseExpiryDate,

                Branch = d.Branch.BranchName,

                Status = d.Status.ToString(),

                HasActiveAssignment = d.VehicleAssignments
                    .Any(a => a.Status == AssignmentStatus.Active),

                ActiveAssignmentId = d.VehicleAssignments
                    .Where(a => a.Status == AssignmentStatus.Active)
                    .Select(a => (int?)a.AssignmentId)
                    .FirstOrDefault(),

                AssignedVehicle = d.VehicleAssignments
                    .Where(a => a.Status == AssignmentStatus.Active)
                    .Select(a =>
                        a.Vehicle.RegistrationNumber
                        + " - "
                        + a.Vehicle.Brand
                        + " "
                        + a.Vehicle.Model)
                    .FirstOrDefault()
            })
            .ToListAsync();
    }

    public async Task<Driver?> GetByIdAsync(int driverId)
    {
        return await _context.Drivers
            .Include(x => x.Branch)
            .Include(x => x.VehicleAssignments)
                .ThenInclude(x => x.Vehicle)
            .FirstOrDefaultAsync(
                x => x.DriverId == driverId);
    }

    public async Task<Vehicle?> GetVehicleByIdAsync(int vehicleId)
    {
        return await _context.Vehicles
            .FirstOrDefaultAsync(
                x => x.VehicleId == vehicleId);
    }

    public async Task<VehicleAssignment?> GetAssignmentByIdAsync(
        int assignmentId)
    {
        return await _context.VehicleAssignments
            .Include(a => a.Driver)
            .Include(a => a.Vehicle)
            .FirstOrDefaultAsync(
                a => a.AssignmentId == assignmentId);
    }

    public async Task<IEnumerable<LookupItemDto>> GetBranchesAsync()
    {
        return await _context.Branches
            .Where(b => b.IsActive)
            .OrderBy(b => b.BranchName)
            .Select(b => new LookupItemDto
            {
                Id = b.BranchId,
                Name = b.BranchName
            })
            .ToListAsync();
    }

    public async Task<bool> EmployeeNumberExistsAsync(
        string employeeNumber)
    {
        return await _context.Drivers
            .AnyAsync(x =>
                x.EmployeeNumber.ToUpper()
                == employeeNumber.ToUpper());
    }

    public async Task<bool> LicenseNumberExistsAsync(
        string licenseNumber)
    {
        return await _context.Drivers
            .AnyAsync(x =>
                x.LicenseNumber.ToUpper()
                == licenseNumber.ToUpper());
    }

    public async Task AddAsync(Driver driver)
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
}