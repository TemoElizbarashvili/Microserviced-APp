using CommandsService.Data;
using CommandsService.Models;
using CommandsService.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Repositories;

public class CommandRepository(ApplicationDbContext context) : ICommandRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task CreateCommandAsync(Command command, int platformId)
    {
        ArgumentNullException.ThrowIfNull(command);

        command.PlatformId = platformId;
        await _context.AddAsync(command);
    }

    public async Task CreatePlatformAsync(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);
        await _context.Platforms.AddAsync(platform);
    }

    public async Task<bool> ExternalPlatformExistsAsync(int externalId)
        => await _context.Platforms.AnyAsync(p => p.ExternalId == externalId);
    
    public async Task<Command?> GetCommandByIdAsync(int platformId, int commandId)
        => await _context.Commands.FirstOrDefaultAsync(c => c.CommandId == commandId && c.PlatformId == platformId);

    public async Task<IEnumerable<Platform>> ListAllPlatformsAsync()
        => await Task.FromResult(_context.Platforms.ToList());

    public async Task<IEnumerable<Command>> ListCommandsForPlatformAsync(int platformId)
        => await Task.FromResult(_context.Commands.Where(c => c.PlatformId == platformId).OrderBy(c => c.Platform.Name));

    public async Task<bool> PlatformExistsAsync(int platformId)
        => await Task.FromResult(_context.Platforms.Any(p => p.PlatformId == platformId));

    public async Task<bool> SaveChangesAsync()
        => (await _context.SaveChangesAsync()) >= 0;
}
