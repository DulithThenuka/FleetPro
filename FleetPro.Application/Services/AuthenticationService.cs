using FleetPro.Application.Interfaces;

namespace FleetPro.Application.Services;

public class AuthenticationService
{
    private readonly IUserRepository _userRepository;

    public AuthenticationService(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task<bool> LoginAsync(
        string username,
        string password)
    {
        var user =
            await _userRepository
            .GetByUsernameAsync(username);


        if(user == null)
            return false;


        return BCrypt.Net.BCrypt.Verify(
            password,
            user.PasswordHash);
    }
}