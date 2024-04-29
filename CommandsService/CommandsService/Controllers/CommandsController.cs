using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using CommandsService.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;
[ApiController]
[Route("platforms/{platformId:int}/commands")]
public class CommandsController(ICommandRepository commandRepository, IMapper mapper) : Controller
{
    private readonly ICommandRepository _commandRepository = commandRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommandReadDto>>> ListCommandsAsync([FromRoute] int platformId)
    {
        if(!(await CheckIfPlatformExists(platformId)))
            return BadRequest($"Platfrom with ID-{platformId} does not exists!");

        var commands = await _commandRepository.ListCommandsForPlatformAsync(platformId);
        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{id:int}", Name = "GetCommand")]
    public async Task<ActionResult<CommandReadDto>> GetCommand([FromRoute] int platformId, [FromRoute] int id)
    {
        if (!(await CheckIfPlatformExists(platformId)))
            return BadRequest($"Platfrom with ID-{platformId} does not exists!");

        try
        {
            var command = await _commandRepository.GetCommandByIdAsync(platformId, id);
            return Ok(_mapper.Map<CommandReadDto>(command));

        }
        catch (Exception ex)
        {
            return BadRequest($"Unable to retrive data! Trace - {ex.Message}");
        }
    }

    [HttpPost("add")]
    public async Task<ActionResult<int>> CreateCommand([FromRoute] int platformId, CommandCreateDto commandDto)
    {
        if (!(await CheckIfPlatformExists(platformId)))
            return BadRequest($"Platfrom with ID-{platformId} does not exists!");

        var command = _mapper.Map<Command>(commandDto);
        try
        {
            await _commandRepository.CreateCommandAsync(command, platformId);
            await _commandRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return Created();
    }


    private async Task<bool> CheckIfPlatformExists(int platformId)
        => await _commandRepository.PlatformExistsAsync(platformId);
}
