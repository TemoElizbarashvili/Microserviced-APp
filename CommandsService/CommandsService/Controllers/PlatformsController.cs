using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[ApiController]
[Route("platforms")]
public class PlatformsController(ICommandRepository commandRepository, IMapper mapper) : Controller
{
    private readonly ICommandRepository _commandRepository = commandRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> ListPlatformsAsync()
    {
        var platforms = await _commandRepository.ListAllPlatformsAsync();
        var platformsReadDtos = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
        return Ok(platformsReadDtos);
    }


    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound POST # Command Service");

        return Ok();
    }
}
