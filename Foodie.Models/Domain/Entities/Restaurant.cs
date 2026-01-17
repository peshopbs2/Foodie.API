using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Models.Domain.Entities
{
    /// <summary>
    /// Represents a restaurant entity
    /// </summary>
    public class Restaurant
    {
        /// <summary>
        /// Gets or sets the unique identifier
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Gets or sets the restaurant's name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the restaurant's address
        /// </summary>
        public string Address { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the restaurant's phone number
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the restaurant's capacity
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Collection of images for the restaurant
        /// </summary>
        public virtual ICollection<RestaurantImage> Images { get; set; } =
            new List<RestaurantImage>();

        /// <summary>
        /// Collection of menus for the restaurant
        /// </summary>
        public virtual ICollection<Menu> Menus { get; set; } =
            new List<Menu>();

        /// <summary>
        /// Collection of owners (users) assigned to this restaurant.
        /// </summary>
        public virtual ICollection<RestaurantOwner> Owners { get; set; } = new List<RestaurantOwner>();

    }
}
