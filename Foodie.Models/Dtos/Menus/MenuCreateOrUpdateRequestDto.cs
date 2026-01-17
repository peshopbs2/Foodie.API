using System;
using System.Collections.Generic;
using Foodie.Models.Dtos.MenuCategories;

namespace Foodie.Models.Dtos.Menus
{
    /// <summary>
    /// Represents a menu create or update request DTO.
    /// </summary>
    public class MenuCreateOrUpdateRequestDto
    {
        /// <summary>
        /// Gets or sets the restaurant id to which the menu belongs.
        /// </summary>
        public Guid RestaurantId { get; set; }

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
    }
}
