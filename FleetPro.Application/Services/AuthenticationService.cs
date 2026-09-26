using FleetPro.Application.DTOs;
using FleetPro.Application.Interfaces;

namespace FleetPro.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;


    public AuthenticationService(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task<LoginResponse> LoginAsync(
        LoginRequest request)
    {
        var user =
            await _userRepository
            .GetByUsernameAsync(request.Username);


        if (user == null)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "User not found."
            };
        }


        if (!user.IsActive)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "User account is inactive."
            };
        }


        var passwordValid =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);


        if (!passwordValid)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid password."
            };
        }


        var role =
            user.UserRoles
            .FirstOrDefault()
            ?.Role
            ?.RoleName;


        return new LoginResponse
        {
            Success = true,
            Message = "Login successful.",
            Username = user.Username,
            Role = role
        };
    }
}