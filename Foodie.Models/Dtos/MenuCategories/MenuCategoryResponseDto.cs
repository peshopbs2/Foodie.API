using System;
using System.Collections.Generic;
using Foodie.Models.Dtos.MenuItems;

namespace Foodie.Models.Dtos.MenuCategories
{
    /// <summary>
    /// Represents a menu category response DTO.
    /// </summary>
    public class MenuCategoryResponseDto
    {
        /// <summary>
        /// Gets or sets the category unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display order inside the menu.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets whether the category is active/visible.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the menu id to which this category belongs.
        /// </summary>
        public Guid MenuId { get; set; }

        /// <summary>
        /// Gets or sets the list of items in this category (optional).
        /// </summary>
        public List<MenuItemResponseDto> Items { get; set; } = [];
    }
}
