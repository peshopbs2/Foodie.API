using AutoMapper;
using Foodie.Models.Domain.Entities;
using Foodie.Models.Dtos.Restaurants;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Foodie.Business.Mappings
{
    /// <summary>
    /// Defines mappings for restaurant DTOs.
    /// </summary>
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            // Entity -> Response DTO
            CreateMap<Restaurant, RestaurantResponseDto>()
                .ForMember(
                    dest => dest.Images,
                    opt => opt.MapFrom(src => src.Images))
                .ForMember(
                    dest => dest.Owners,
                    opt => opt.MapFrom(src => src.Owners.Select(x => x.User)));

            // RestaurantImage -> RestaurantImageResponseDto
            CreateMap<RestaurantImage, RestaurantImageResponseDto>();

            // User -> RestaurantOwnerResponseDto
            CreateMap<IdentityUser, RestaurantOwnerResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            // Create request -> Entity (only core fields)
            CreateMap<RestaurantCreateRequestDto, Restaurant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Owners, opt => opt.Ignore());

            // Update request -> Entity (only core fields)
            // We ignore image + owners management because that is handled manually in the service.
            CreateMap<RestaurantUpdateRequestDto, Restaurant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Owners, opt => opt.Ignore());
        }
    }
}
