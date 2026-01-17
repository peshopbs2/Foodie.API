using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Foodie.Models.Dtos.Restaurants
{
    /// <summary>
    /// Represents a restaurant update request DTO.
    /// </summary>
    public class RestaurantUpdateRequestDto
    {
        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public int Capacity { get; set; }

        /// <summary>
        /// Optional new images uploaded by the user (multipart/form-data).
        /// </summary>
        public List<IFormFile> NewImages { get; set; } = [];

        /// <summary>
        /// Optional index (within NewImages) to be marked as main.
        /// </summary>
        public int? MainImageIndex { get; set; }

        /// <summary>
        /// Replace owners with these IDs (optional).
        /// </summary>
        public List<string> OwnerIds { get; set; } = [];

        /// <summary>
        /// Optional list of image IDs to delete.
        /// </summary>
        public List<Guid> DeleteImageIds { get; set; } = [];
    }
}
