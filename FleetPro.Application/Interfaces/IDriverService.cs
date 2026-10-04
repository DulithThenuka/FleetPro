using FleetPro.Application.DTOs;

namespace FleetPro.Application.Interfaces;

public interface IDriverService
{
    Task<List<DriverListItemDto>> GetAllAsync();

    Task<(bool Success, string Message)>
        CreateAsync(
            CreateDriverRequest request);

    Task<(bool Success, string Message)>
        AssignVehicleAsync(
            AssignVehicleRequest request);

    Task<IEnumerable<LookupItemDto>> GetBranchesAsync();

    Task CompleteAssignmentAsync(
    int assignmentId,
    DateTime endDate,
    decimal? endMileage,
    string? notes);

    Task<DriverListItemDto?> GetByIdAsync(int driverId);
}