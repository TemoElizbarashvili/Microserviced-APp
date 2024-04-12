using Microsoft.EntityFrameworkCore;
using platformService.DataContext;
using platformService.Models;
using platformService.Repositories.Contracts;

namespace platformService.Repositories;

public class PlatformRepository(ApplicationDbContext context) : IPlatformRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task CreatePlatformAsync(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);
        await _context.Platforms.AddAsync(platform);
    }

    public async Task<Platform?> GetPlatformByIdAsync(int platformId)
        => await _context.Platforms.FirstOrDefaultAsync(plat => plat.PlatformId == platformId);

    public async Task<IEnumerable<Platform>> ListPlatformsAsync()
        => await Task.FromResult(_context.Platforms);

    public async Task<bool> SaveChangesAsync()
        => (await _context.SaveChangesAsync()) >= 0;
}