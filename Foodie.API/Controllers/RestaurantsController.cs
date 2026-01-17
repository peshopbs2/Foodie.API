using Foodie.Business.Services.Interfaces;
using Foodie.Models.Dtos.Restaurants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Foodie.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IAuthorizationService _authorizationService;

        public RestaurantsController(
            IRestaurantService restaurantService,
            IAuthorizationService authorizationService)
        {
            _restaurantService = restaurantService ?? throw new ArgumentNullException(nameof(restaurantService));
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        /// <summary>
        /// Get all restaurants
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantResponseDto>>> GetAllAsync()
        {
            IEnumerable<RestaurantResponseDto> restaurants = await _restaurantService.GetAllAsync();
            return Ok(restaurants);
        }

        /// <summary>
        /// Get restaurants owned by the currently logged-in user (RestaurantOwner only).
        /// </summary>
        [HttpGet("my")]
        [Authorize(Roles = "RestaurantOwner")]
        public async Task<ActionResult<IEnumerable<RestaurantResponseDto>>> GetMyRestaurantsAsync()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            IEnumerable<RestaurantResponseDto> restaurants = await _restaurantService.GetByOwnerIdAsync(userId);
            return Ok(restaurants);
        }

        /// <summary>
        /// Get restaurant by id (Admin or owner of that restaurant).
        /// </summary>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        public async Task<ActionResult<RestaurantResponseDto>> GetByIdAsync(Guid id)
        {
            AuthorizationResult authResult = await _authorizationService
                .AuthorizeAsync(User, id, "ManageRestaurant");

            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            RestaurantResponseDto? restaurant = await _restaurantService.GetByIdAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }

        /// <summary>
        /// Search restaurants by address (public endpoint).
        /// </summary>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RestaurantResponseDto>>> SearchByAddressAsync([FromQuery] string address)
        {
            IEnumerable<RestaurantResponseDto> restaurants = await _restaurantService.SearchByAddressAsync(address);
            return Ok(restaurants);
        }

        /// <summary>
        /// Create restaurant (Admin or RestaurantOwner).
        /// Use multipart/form-data when uploading images.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<RestaurantResponseDto>> CreateAsync([FromForm] RestaurantCreateRequestDto model)
        {
            // Optional rule:
            // If RestaurantOwner creates a restaurant, enforce that they become owner automatically.
            // You can do it either here or inside the service.

            if (User.IsInRole("RestaurantOwner"))
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    model.OwnerIds ??= [];
                    if (!model.OwnerIds.Contains(userId))
                    {
                        model.OwnerIds.Add(userId);
                    }
                }
            }

            RestaurantResponseDto created = await _restaurantService.CreateAsync(model);

            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = created.Id },
                created);
        }

        /// <summary>
        /// Update restaurant (Admin or owner of that restaurant).
        /// Use multipart/form-data when uploading new images.
        /// </summary>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<RestaurantResponseDto>> UpdateAsync(Guid id, [FromForm] RestaurantUpdateRequestDto model)
        {
            AuthorizationResult authResult = await _authorizationService
                .AuthorizeAsync(User, id, "ManageRestaurant");

            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            RestaurantResponseDto updated = await _restaurantService.UpdateAsync(id, model);
            return Ok(updated);
        }

        /// <summary>
        /// Delete restaurant (Admin or owner of that restaurant).
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        public async Task<ActionResult<RestaurantResponseDto>> DeleteAsync(Guid id)
        {
            AuthorizationResult authResult = await _authorizationService
                .AuthorizeAsync(User, id, "ManageRestaurant");

            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            RestaurantResponseDto deleted = await _restaurantService.DeleteAsync(id);
            return Ok(deleted);
        }
    }
}
