using System.ComponentModel.DataAnnotations;

namespace platformService.Models;

public class Platform
{
    public int PlatformId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Publisher { get; set; } = string.Empty;
    [Required]
    public string Cost { get; set; } = string.Empty;
}