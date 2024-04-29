using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using CommandsService.Repositories.Contracts;
using System.Text.Json;

namespace CommandsService.EventProcessing;

public class EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper) : IEventProcessor
{
    private const string publishedEventMessage = "Platform_Published";
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IMapper _mapper = mapper;

    public async Task ProcessEvent(string message)
    {
        var eventType = DetermineEventType(message);


        switch (eventType)
        {
            case EventType.PlatformPublished:
                await addPlatform(message);
                Console.WriteLine("--> Event Determined as a published!");
                break;
            default:
                Console.WriteLine("--> Can not Determine the event!");
                break;
        }
    }

    private EventType DetermineEventType(string message)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);

        return eventType?.Event switch
        {
            publishedEventMessage => EventType.PlatformPublished,
            _ => EventType.Undetermined,
        };
    }

    private async Task addPlatform(string PlatformPublishedMessage)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();

            var platformPublishDto = JsonSerializer.Deserialize<PlatformPublishedDto>(PlatformPublishedMessage);

            try
            {
                var platform = _mapper.Map<Platform>(platformPublishDto);

                if (!(await repository.ExternalPlatformExistsAsync(platform.ExternalId)))
                {
                    await repository.CreatePlatformAsync(platform);
                    await repository.SaveChangesAsync();
                    Console.WriteLine($"--> Platform Added successfully! :)");

                }
                else
                    Console.WriteLine($"--> Given platform already exists! {platform.Name}");


            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Error occuired while adding platform to DB, Trace -- {ex.Message}");
            }
        }
    }
}

enum EventType
{
    PlatformPublished,
    Undetermined
}
