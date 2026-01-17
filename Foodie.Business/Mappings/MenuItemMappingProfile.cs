using AutoMapper;
using Foodie.Models.Domain.Entities;
using Foodie.Models.Dtos.MenuItems;

namespace Foodie.Business.Mappings
{
    /// <summary>
    /// Defines mappings for menu item DTOs.
    /// </summary>
    public class MenuItemMappingProfile : Profile
    {
        public MenuItemMappingProfile()
        {
            CreateMap<MenuItem, MenuItemResponseDto>()
                .ForMember(
                    dest => dest.MenuCategoryName,
                    opt => opt.MapFrom(src => src.MenuCategory.Name));

            CreateMap<MenuItemCreateOrUpdateRequestDto, MenuItem>();
        }
    }
}
