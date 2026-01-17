using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Foodie.Models.Dtos.Restaurants
{
    /// <summary>
    /// Represents restaurant owner response DTO.
    /// </summary>
    public class RestaurantOwnerResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
