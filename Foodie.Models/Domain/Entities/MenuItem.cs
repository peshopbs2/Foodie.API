using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Models.Domain.Entities
{
    /// <summary>
    /// Represents an item inside a menu category.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Gets or sets the unique identifier.
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
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the item is currently available (in stock).
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Gets or sets the display order inside the category.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Foreign key to the menu category.
        /// </summary>
        public Guid MenuCategoryId { get; set; }

        /// <summary>
        /// Navigation property to the menu category.
        /// </summary>
        public virtual MenuCategory MenuCategory { get; set; } = null!;
    }
}
