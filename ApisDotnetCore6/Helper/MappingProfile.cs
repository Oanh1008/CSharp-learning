using ApisDotnetCore6.Data;
using ApisDotnetCore6.Dto;
using AutoMapper;

namespace ApisDotnetCore6.Helper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserReadDto>()
            .ForMember(
                dest => dest.FullName,
                opt 
                    => opt.MapFrom(nms => $"{nms.FirstName} {nms.LastName}"));
               

        CreateMap<UserCreateDto, User>();
    }
}