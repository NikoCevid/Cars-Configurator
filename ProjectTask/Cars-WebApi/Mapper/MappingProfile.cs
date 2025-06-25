using AutoMapper;
using Cars.DTO;
using Dao.Models;



namespace Cars.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CarComponent, CarComponentDTO>().ReverseMap();
            CreateMap<ComponentType, ComponentTypeDTO>().ReverseMap();
            CreateMap<CarComponentCompatibility, ComponentCompatibilityDTO>().ReverseMap();
            CreateMap<Configuration, ConfigurationDTO>().ReverseMap();
            CreateMap<ConfigurationCarComponent, ConfigurationCarComponentDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();

            // Create-only i pomoćni DTO-i
            CreateMap<CreateCarComponentDTO, CarComponent>();
            CreateMap<CreateComponentTypeDTO, ComponentType>();
            CreateMap<CreateCompabilityDTO, CarComponentCompatibility>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UserLoginDTO, User>();
            CreateMap<ChangePasswordDTO, User>();
        }
    }
}
