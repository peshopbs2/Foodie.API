using Foodie.Models.Dtos.Menus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foodie.Business.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for managing menu data operations.
    /// </summary>
    public interface IMenuService
    {
        /// <summary>
        /// Retrieves all available menus.
        /// </summary>
        /// <returns>A collection of <see cref="MenuResponseDto"/> objects.</returns>
        Task<IEnumerable<MenuResponseDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a specific menu by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the menu.</param>
        /// <returns>A <see cref="MenuResponseDto"/> if found; otherwise, <see langword="null"/>.</returns>
        Task<MenuResponseDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all menus for a given restaurant.
        /// </summary>
        /// <param name="restaurantId">The restaurant identifier.</param>
        /// <returns>A collection of <see cref="MenuResponseDto"/> objects.</returns>
        Task<IEnumerable<MenuResponseDto>> GetByRestaurantIdAsync(Guid restaurantId);

        /// <summary>
        /// Creates a new menu entry in the system.
        /// </summary>
        /// <param name="model">The request DTO containing the menu details.</param>
        /// <returns>The created <see cref="MenuResponseDto"/>.</returns>
        Task<MenuResponseDto> CreateAsync(MenuCreateOrUpdateRequestDto model);

        /// <summary>
        /// Updates the details of an existing menu.
        /// </summary>
        /// <param name="id">The unique identifier of the menu to update.</param>
        /// <param name="model">The request DTO containing the updated menu details.</param>
        /// <returns>The updated <see cref="MenuResponseDto"/>.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no menu exists with the provided <paramref name="id"/>.
        /// </exception>
        Task<MenuResponseDto> UpdateAsync(Guid id, MenuCreateOrUpdateRequestDto model);

        /// <summary>
        /// Removes a menu from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the menu to delete.</param>
        /// <returns>The <see cref="MenuResponseDto"/> of the deleted menu.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no menu exists with the provided <paramref name="id"/>.
        /// </exception>
        Task<MenuResponseDto> DeleteAsync(Guid id);
    }
}
