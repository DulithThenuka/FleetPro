using FleetPro.Application.DTOs;

namespace FleetPro.Application.Interfaces;

public interface IVehicleService
{
    Task<List<VehicleListItemDto>> GetAllAsync();

    Task<(bool Success, string Message)> CreateAsync(
        CreateVehicleRequest request);
}