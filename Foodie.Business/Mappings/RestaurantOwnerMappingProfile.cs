using AutoMapper;
using Foodie.Models.Dtos.Restaurants;
using Microsoft.AspNetCore.Identity;

namespace Foodie.Business.Mappings
{
    /// <summary>
    /// Defines mappings for restaurant owner DTOs.
    /// </summary>
    public class RestaurantOwnerMappingProfile : Profile
    {
        public RestaurantOwnerMappingProfile()
        {
            CreateMap<IdentityUser, RestaurantOwnerResponseDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
