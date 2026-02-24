using _2.Application.Entities;
using ArchitecutreMachin.Models;
using ArchitecutreMachin.Models.CarFeatures;
using ArchitecutreMachin.Models.Cars;
using ArchitecutreMachin.Models.Users;
using AutoMapper;

namespace ArchitecutreMachin.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserSelectDto>().ReverseMap();
            CreateMap<UserDto, User>();

            // Car
            CreateMap<Car, CarSelectDto>().ReverseMap();   
            CreateMap<CarDto, Car>()
                .ForMember(d => d.Features, opt => opt.Ignore());

            // CarFeature
            CreateMap<CarFeature, CarFeatureSelectDto>().ReverseMap();
            CreateMap<CarFeatureDto, CarFeature>();
        }
    }
}
