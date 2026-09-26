using FleetPro.Domain.Entities;

namespace FleetPro.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
}