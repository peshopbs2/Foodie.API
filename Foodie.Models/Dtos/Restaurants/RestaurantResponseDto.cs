using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Foodie.Models.Dtos.Restaurants
{
    /// <summary>
    /// Represents restaurant response DTO.
    /// </summary>
    public class RestaurantResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public List<RestaurantImageResponseDto> Images { get; set; } = [];

        public List<RestaurantOwnerResponseDto> Owners { get; set; } = [];
    }
}
