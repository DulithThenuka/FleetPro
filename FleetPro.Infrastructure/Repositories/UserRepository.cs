using FleetPro.Application.Interfaces;
using FleetPro.Domain.Entities;
using FleetPro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace FleetPro.Infrastructure.Repositories;


public class UserRepository : IUserRepository
{
    private readonly FleetProDbContext _context;


    public UserRepository(
        FleetProDbContext context)
    {
        _context = context;
    }


    public async Task<User?> GetByUsernameAsync(
        string username)
    {
        return await _context.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(
                x => x.Username == username);
    }
}