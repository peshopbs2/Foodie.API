using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Models.Domain.Entities
{
    /// <summary>
    /// Represents a category inside a menu (e.g., Starters, Sushi Rolls, Drinks).
    /// </summary>
    public class MenuCategory
    {
        /// <summary>
        /// Gets or sets the unique identifier.
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
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Foreign key to the menu.
        /// </summary>
        public Guid MenuId { get; set; }

        /// <summary>
        /// Navigation property to the menu.
        /// </summary>
        public virtual Menu Menu { get; set; } = null!;

        /// <summary>
        /// Collection of items in the category.
        /// </summary>
        public virtual ICollection<MenuItem> Items { get; set; } = new List<MenuItem>();
    }
}
