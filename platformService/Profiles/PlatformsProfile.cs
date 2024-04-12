using AutoMapper;
using platformService.Dtos;
using platformService.Models;

namespace platformService.Profiles;

public class PlatformsProfile : Profile
{
    public PlatformsProfile()
    {
        // Source -> Target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
    }
}
