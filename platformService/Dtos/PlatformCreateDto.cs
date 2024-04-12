using System.ComponentModel.DataAnnotations;

namespace platformService.Dtos;

public record PlatformCreateDto
{
    [Required]
    public string Name = string.Empty;
    [Required]
    public string Publisher = string.Empty;
    [Required]
    public string Cost = string.Empty;
}
