using AutoMapper;
using Cars.DTO;
using Dao.Models;

namespace Cars.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --- Bidirectional DTOs ---
            CreateMap<CarComponent, CarComponentDTO>().ReverseMap();
            CreateMap<ComponentType, ComponentTypeDTO>().ReverseMap();
            CreateMap<CarComponentCompatibility, ComponentCompatibilityDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();

            // --- Configuration with nested ConfigurationCarComponents ---
            CreateMap<Configuration, ConfigurationDTO>()
                .ForMember(dest => dest.ConfigurationCarComponents, opt => opt.MapFrom(src => src.ConfigurationCarComponents))
                .ReverseMap()
                .ForMember(dest => dest.ConfigurationCarComponents, opt => opt.MapFrom(src => src.ConfigurationCarComponents));

            // --- ConfigurationCarComponentDTO for READ (includes CarComponent.Name) ---
            CreateMap<ConfigurationCarComponent, ConfigurationCarComponentDTO>()
                .ForMember(dest => dest.CarComponentName, opt => opt.MapFrom(src => src.CarComponent.Name))
                .ReverseMap()
                .ForMember(dest => dest.CarComponent, opt => opt.Ignore()); // prevent EF from trying to insert nested CarComponent

            // --- Create-only DTOs ---
            CreateMap<CreateCarComponentDTO, CarComponent>();
            CreateMap<CreateComponentTypeDTO, ComponentType>();
            CreateMap<CreateCompabilityDTO, CarComponentCompatibility>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UserLoginDTO, User>();
            CreateMap<ChangePasswordDTO, User>();

            // --- Compatibility View DTO ---
            CreateMap<CarComponent, ComponentCompatibilityDTO>()
                .ForMember(dest => dest.ComponentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ComponentName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CompatibleWith, opt => opt.MapFrom(src =>
                    src.CarComponentCompatibilityCarComponentId1Navigations
                        .Select(c => new CompatibleComponentDTO
                        {
                            Id = c.CarComponentId2Navigation.Id,
                            Name = c.CarComponentId2Navigation.Name
                        })
                        .ToList()));
        }
    }
}
