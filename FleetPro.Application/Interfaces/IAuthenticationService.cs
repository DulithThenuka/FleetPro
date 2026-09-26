using FleetPro.Application.DTOs;

namespace FleetPro.Application.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponse> LoginAsync(
        LoginRequest request);
}