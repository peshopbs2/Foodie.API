using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Models.Domain.Entities
{
    /// <summary>
    /// Represents an image associated with a restaurant
    /// </summary>
    public class RestaurantImage
    {
        public Guid Id { get; set; }

        /// <summary>
        /// The file system path or URL to the image
        /// </summary>
        [Required]
        public string ImagePath { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if this is the primary cover image
        /// </summary>
        public bool IsMainImage { get; set; }

        /// <summary>
        /// Foreign key for the Restaurant
        /// </summary>
        public Guid RestaurantId { get; set; }

        /// <summary>
        /// Navigation property
        /// </summary>
        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurant Restaurant { get; set; } = null!;
    }
}
