using FleetPro.Application.DTOs;

namespace FleetPro.Application.Interfaces;

public interface IDriverService
{
    Task<IEnumerable<DriverListItemDto>> GetAllAsync();

    Task<DriverListItemDto?> GetByIdAsync(
        int driverId);

    Task<IEnumerable<LookupItemDto>> GetBranchesAsync();

    Task<(bool Success, string Message)> CreateAsync(
        CreateDriverRequest request);

    Task<(bool Success, string Message)> AssignVehicleAsync(
        AssignVehicleRequest request);

    Task CompleteAssignmentAsync(
        int assignmentId,
        DateTime endDate,
        decimal? endMileage,
        string? notes);

    Task UpdateAsync(
        UpdateDriverRequest request);
}