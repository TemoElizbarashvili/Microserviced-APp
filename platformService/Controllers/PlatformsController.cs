using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using platformService.AsyncDataServices.Messaging;
using platformService.Dtos;
using platformService.Models;
using platformService.Repositories.Contracts;
using platformService.SyncDataServices.Http;

namespace platformService.Controllers;

[Route("[controller]")]
[ApiController]
public class PlatformsController(IPlatformRepository platformRepository, IMapper mapper, ICommandDataClient commandDataclient, IMessageBusClient messageBusClient) : Controller
{
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ICommandDataClient _commandDataclient = commandDataclient;
    private readonly IMessageBusClient _messageBusClient = messageBusClient;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> ListPlatformsAsync()
    {
        var platforms = await _platformRepository.ListPlatformsAsync();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    [HttpGet("{id:int}", Name = "GetById")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<PlatformReadDto>> GetById([FromRoute] int id)
    {
        var platform = await _platformRepository.GetPlatformByIdAsync(id);
        if (platform is null)
            return NotFound($"Can not found item with Id - {id}");
        return Ok(_mapper.Map<PlatformReadDto>(platform));
    }

    [HttpPost("add")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<int>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        var platform = _mapper.Map<Platform>(platformCreateDto);
        try
        {
            await _platformRepository.CreatePlatformAsync(platform);
            await _platformRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);

        try
        {
            await _commandDataclient.SendPlatformToCommand(platformReadDto);

            var publishDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
            publishDto.Event = "Platform_Published";
            await _messageBusClient.PublishNewPlatform(publishDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Something went wrong while posting async, Trace - {ex.Message}");
        }

        return CreatedAtRoute(nameof(GetById), new { Id = platformReadDto.PlatformId }, platformReadDto.PlatformId);
    }
}
