using FleetPro.Application.DTOs;

namespace FleetPro.Application.Interfaces;

public interface IVehicleService
{
    Task<List<VehicleListItemDto>> GetAllAsync();

    Task<List<LookupItemDto>> GetBranchesAsync();

    Task<List<LookupItemDto>> GetVehicleTypesAsync();

    Task<(bool Success, string Message)> CreateAsync(
        CreateVehicleRequest request);
}