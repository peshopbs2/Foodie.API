using AutoMapper;
using Foodie.Models.Domain.Entities;
using Foodie.Models.Dtos.Menus;

namespace Foodie.Business.Mappings
{
    /// <summary>
    /// Defines mappings for menu DTOs.
    /// </summary>
    public class MenuMappingProfile : Profile
    {
        public MenuMappingProfile()
        {
            CreateMap<Menu, MenuResponseDto>()
                .ForMember(
                    dest => dest.RestaurantName,
                    opt => opt.MapFrom(src => src.Restaurant != null ? src.Restaurant.Name : string.Empty));

            CreateMap<MenuCreateOrUpdateRequestDto, Menu>();
        }
    }
}
