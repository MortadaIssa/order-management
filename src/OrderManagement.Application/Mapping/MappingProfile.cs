using AutoMapper;
using OrderManagement.Application.DTOs;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<RegisterUserDto, User>();
        }
    }
}
