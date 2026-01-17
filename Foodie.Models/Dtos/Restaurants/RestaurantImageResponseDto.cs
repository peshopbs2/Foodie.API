using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Foodie.Models.Dtos.Restaurants
{
    /// <summary>
    /// Represents a restaurant image response DTO.
    /// </summary>
    public class RestaurantImageResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the image.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the relative file path or URL to the image.
        /// </summary>
        public string ImagePath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this image is the main/cover image.
        /// </summary>
        public bool IsMainImage { get; set; }
    }
}
