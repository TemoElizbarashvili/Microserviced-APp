using platformService.Dtos;

namespace platformService.AsyncDataServices.Messaging;

public interface IMessageBusClient
{
    Task PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
}
