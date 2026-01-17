using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Models.Dtos.MenuItems
{
    /// <summary>
    /// Represents a menu item response DTO.
    /// </summary>
    public class MenuItemResponseDto
    {
        /// <summary>
        /// Gets or sets the item unique identifier.
        /// </summary>
        public Guid Id { get; set; }

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
        /// Gets or sets whether the item is active/visible.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets whether the item is currently available (in stock).
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the display order inside the category.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the menu category id.
        /// </summary>
        public Guid MenuCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the menu category name (optional convenience field).
        /// </summary>
        public string MenuCategoryName { get; set; } = string.Empty;
    }
}
