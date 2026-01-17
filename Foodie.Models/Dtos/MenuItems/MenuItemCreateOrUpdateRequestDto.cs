using System;
using System.Collections.Generic;

namespace Foodie.Models.Dtos.MenuItems
{
    /// <summary>
    /// Represents a menu item create or update request DTO.
    /// </summary>
    public class MenuItemCreateOrUpdateRequestDto
    {
        /// <summary>
        /// Gets or sets the menu category id.
        /// </summary>
        public Guid MenuCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the item name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the item description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the item price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets whether the item is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the item is available (in stock).
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Gets or sets the display order inside the category.
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}