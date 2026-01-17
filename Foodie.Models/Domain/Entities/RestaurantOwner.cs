using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Models.Domain.Entities
{
    /// <summary>
    /// Represents the link between a restaurant and a user (owner).
    /// </summary>
    public class RestaurantOwner
    {
        /// <summary>
        /// Foreign key to the Restaurant.
        /// </summary>
        public Guid RestaurantId { get; set; }

        /// <summary>
        /// Navigation property to the Restaurant.
        /// </summary>
        public virtual Restaurant Restaurant { get; set; } = null!;

        /// <summary>
        /// Foreign key to the User (IdentityUser uses string Id by default).
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the User.
        /// </summary>
        public virtual IdentityUser User { get; set; } = null!;
    }
}
