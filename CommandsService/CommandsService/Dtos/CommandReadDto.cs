using CommandsService.Models;

namespace CommandsService.Dtos;

public class CommandReadDto
{
    public int CommandId { get; set; }
    public required string HowTo { get; set; }
    public required string CommandLine { get; set; }
    public required string PlatformId { get; set; }
}
