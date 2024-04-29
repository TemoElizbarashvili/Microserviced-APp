namespace CommandsService.Dtos;

public class PlatformReadDto
{
    public int PlatformId { get; set; }
    public required string Name { get; set; } = string.Empty;
}
