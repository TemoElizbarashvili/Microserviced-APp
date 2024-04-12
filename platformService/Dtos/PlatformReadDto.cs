namespace platformService.Dtos;

public record PlatformReadDto
{
    public int PlatformId;
    public string Name = string.Empty;
    public string Publisher = string.Empty;
    public string Cost  = string.Empty;
}
