using AutoMapper;
using Cars.DTO;
using Dao.Models;

namespace Cars.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Bidirectional DTOs
            CreateMap<CarComponent, CarComponentDTO>().ReverseMap();
            CreateMap<ComponentType, ComponentTypeDTO>().ReverseMap();
            CreateMap<CarComponentCompatibility, ComponentCompatibilityDTO>().ReverseMap();
            CreateMap<Configuration, ConfigurationDTO>().ReverseMap();

            // Popravljeno mapiranje - izbjegava mapiranje navigacijskih svojstava
            CreateMap<ConfigurationCarComponent, ConfigurationCarComponentDTO>()
                .ForMember(dest => dest.CarComponentName, opt => opt.MapFrom(src => src.CarComponent.Name))
                .ReverseMap()
                .ForMember(dest => dest.CarComponent, opt => opt.Ignore()); // <- SPRIJEČI INSERT!

            CreateMap<User, UserDTO>().ReverseMap();

            // Create-only DTOs
            CreateMap<CreateCarComponentDTO, CarComponent>();
            CreateMap<CreateComponentTypeDTO, ComponentType>();
            CreateMap<CreateCompabilityDTO, CarComponentCompatibility>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UserLoginDTO, User>();
            CreateMap<ChangePasswordDTO, User>();
        }
    }
}
