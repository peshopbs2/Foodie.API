using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Foodie.Models.Dtos.Restaurants
{
    /// <summary>
    /// Represents a restaurant create request DTO.
    /// </summary>
    public class RestaurantCreateRequestDto
    {
        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public int Capacity { get; set; }

        /// <summary>
        /// Images uploaded by the user (multipart/form-data).
        /// </summary>
        public List<IFormFile> Images { get; set; } = [];

        /// <summary>
        /// Index of the image to be marked as main.
        /// </summary>
        public int MainImageIndex { get; set; } = 0;

        /// <summary>
        /// List of user IDs selected as owners.
        /// </summary>
        public List<string> OwnerIds { get; set; } = [];
    }

}
