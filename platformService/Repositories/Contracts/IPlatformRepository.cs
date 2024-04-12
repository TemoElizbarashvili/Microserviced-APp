using platformService.Models;

namespace platformService.Repositories.Contracts;

public interface IPlatformRepository
{
    Task<bool> SaveChangesAsync();
    Task<IEnumerable<Platform>> ListPlatformsAsync();
    Task<Platform?> GetPlatformByIdAsync(int platformId);
    Task CreatePlatformAsync(Platform platform);
}
