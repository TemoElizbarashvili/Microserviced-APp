using platformService.Dtos;

namespace platformService.SyncDataServices.Http;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto platformReadDto);
}
