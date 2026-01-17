using System;
using System.Collections.Generic;
using Foodie.Models.Dtos.MenuCategories;

namespace Foodie.Models.Dtos.Menus
{
    /// <summary>
    /// Represents a menu response DTO.
    /// </summary>
    public class MenuResponseDto
    {
        /// <summary>
        /// Gets or sets the menu id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the restaurant id.
        /// </summary>
        public Guid RestaurantId { get; set; }

        /// <summary>
        /// Gets or sets the restaurant name (optional convenience field).
        /// </summary>
        public string RestaurantName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the menu name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start of the menu activity period.
        /// </summary>
        public DateTime ActiveFrom { get; set; }

        /// <summary>
        /// Gets or sets the end of the menu activity period.
        /// </summary>
        public DateTime? ActiveTo { get; set; }

        /// <summary>
        /// Gets or sets the list of categories in this menu (optional).
        /// </summary>
        public List<MenuCategoryResponseDto> Categories { get; set; } = [];
    }
}
