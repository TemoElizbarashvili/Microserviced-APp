using CommandsService.Models;

namespace CommandsService.Repositories.Contracts;

public interface ICommandRepository
{
    Task<bool> SaveChangesAsync();

    // Platform related
    Task<IEnumerable<Platform>> ListAllPlatformsAsync();
    Task CreatePlatformAsync(Platform platform);
    Task<bool> PlatformExistsAsync(int platformId);
    Task<bool> ExternalPlatformExistsAsync(int externalId);

    // Commands Related
    Task<IEnumerable<Command>> ListCommandsForPlatformAsync(int platformId);
    Task<Command?> GetCommandByIdAsync(int platformId, int commandId);
    Task CreateCommandAsync(Command command, int platformId);

}
