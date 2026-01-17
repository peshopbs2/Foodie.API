using Foodie.Models.Dtos.Restaurants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foodie.Business.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for managing restaurant data operations.
    /// </summary>
    public interface IRestaurantService
    {
        /// <summary>
        /// Retrieves all available restaurants.
        /// </summary>
        /// <returns>A collection of <see cref="RestaurantResponseDto"/> objects.</returns>
        Task<IEnumerable<RestaurantResponseDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a specific restaurant by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the restaurant.</param>
        /// <returns>
        /// A <see cref="RestaurantResponseDto"/> if found; otherwise, <see langword="null"/>.
        /// </returns>
        Task<RestaurantResponseDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all available restaurants owned by the given owner.
        /// </summary>
        /// <param name="ownerId">The unique identifier of the restaurant owner.</param>
        /// <returns>A collection of <see cref="RestaurantResponseDto"/> objects.</returns>
        Task<IEnumerable<RestaurantResponseDto>> GetByOwnerIdAsync(string ownerId);

        /// <summary>
        /// Searches for restaurants that match a specific address or location string.
        /// </summary>
        /// <param name="address">The address or partial address to search for.</param>
        /// <returns>A collection of restaurants matching the search criteria.</returns>
        Task<IEnumerable<RestaurantResponseDto>> SearchByAddressAsync(string address);

        /// <summary>
        /// Creates a new restaurant entry in the system.
        /// </summary>
        /// <param name="model">The request DTO containing the details of the restaurant to create.</param>
        /// <returns>The created <see cref="RestaurantResponseDto"/>.</returns>
        Task<RestaurantResponseDto> CreateAsync(RestaurantCreateRequestDto model);

        /// <summary>
        /// Updates the details of an existing restaurant.
        /// </summary>
        /// <param name="id">The unique identifier of the restaurant to update.</param>
        /// <param name="model">The request DTO containing the updated restaurant details.</param>
        /// <returns>The updated <see cref="RestaurantResponseDto"/>.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no restaurant exists with the provided <paramref name="id"/>.
        /// </exception>
        Task<RestaurantResponseDto> UpdateAsync(Guid id, RestaurantUpdateRequestDto model);

        /// <summary>
        /// Removes a restaurant from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the restaurant to delete.</param>
        /// <returns>The <see cref="RestaurantResponseDto"/> of the deleted restaurant.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no restaurant exists with the provided <paramref name="id"/>.
        /// </exception>
        Task<RestaurantResponseDto> DeleteAsync(Guid id);
    }
}
