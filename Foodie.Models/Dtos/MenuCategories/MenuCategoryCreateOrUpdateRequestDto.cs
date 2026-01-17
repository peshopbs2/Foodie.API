using System;
using System.Collections.Generic;
using Foodie.Models.Dtos.MenuItems;

namespace Foodie.Models.Dtos.MenuCategories
{
    /// <summary>
    /// Represents a menu category create or update request DTO.
    /// </summary>
    public class MenuCategoryCreateOrUpdateRequestDto
    {
        /// <summary>
        /// Gets or sets the menu id to which the category will belong.
        /// </summary>
        public Guid MenuId { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display order inside the menu.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets whether the category is active.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
