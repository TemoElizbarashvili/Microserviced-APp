using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models;

public class Platform
{
    [Key]
    public int PlatformId { get; set; }
    public required int ExternalId { get; set; }
    public required string Name { get; set; } = string.Empty;

    public ICollection<Command>? Commands { get; set;}
}
