using AutoMapper;
using Foodie.Models.Domain.Entities;
using Foodie.Models.Dtos.MenuCategories;

namespace Foodie.Business.Mappings
{
    /// <summary>
    /// Defines mappings for menu category DTOs.
    /// </summary>
    public class MenuCategoryMappingProfile : Profile
    {
        public MenuCategoryMappingProfile()
        {
            CreateMap<MenuCategory, MenuCategoryResponseDto>();

            CreateMap<MenuCategoryCreateOrUpdateRequestDto, MenuCategory>();
        }
    }
}
