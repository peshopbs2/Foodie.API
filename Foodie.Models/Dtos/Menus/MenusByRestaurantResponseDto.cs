using System;
using System.Collections.Generic;
using Foodie.Models.Dtos.MenuCategories;

namespace Foodie.Models.Dtos.Menus
{
    /// <summary>
    /// Represents a response DTO for listing menus by restaurant id.
    /// </summary>
    public class MenusByRestaurantResponseDto
    {
        public string RestaurantName { get; set; } = string.Empty;

        public List<MenuResponseDto> Menus { get; set; } = [];
    }
}
