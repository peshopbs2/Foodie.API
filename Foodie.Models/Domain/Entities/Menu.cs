using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Models.Domain.Entities
{
    /// <summary>
    /// Represents a menu for a restaurant (e.g., Main, Sushi, Drinks) with an activity period.
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the menu name (e.g., Main Menu, Sushi Menu, Drinks).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start of the menu activity period (inclusive).
        /// </summary>
        public DateTime ActiveFrom { get; set; }

        /// <summary>
        /// Gets or sets the end of the menu activity period (inclusive). Null means no end date.
        /// </summary>
        public DateTime? ActiveTo { get; set; }

        /// <summary>
        /// Foreign key to the restaurant.
        /// </summary>
        public Guid RestaurantId { get; set; }

        /// <summary>
        /// Navigation property to the restaurant.
        /// </summary>
        public virtual Restaurant Restaurant { get; set; } = null!;

        /// <summary>
        /// Navigation property to the categories.
        /// </summary>
        public virtual ICollection<MenuCategory> Categories { get; set; } = new List<MenuCategory>();

    }
}
