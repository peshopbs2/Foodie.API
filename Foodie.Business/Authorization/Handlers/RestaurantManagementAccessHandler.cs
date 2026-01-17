using Foodie.Business.Authorization.Requirements;
using Foodie.Business.Repositories.Interfaces;
using Foodie.Models.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Business.Authorization.Handlers
{
    public class RestaurantManagementAccessHandler : AuthorizationHandler<RestaurantManagementAccessRequirement, Guid>
    {
        private readonly IRepository<Restaurant> _restaurantRepository;

        public RestaurantManagementAccessHandler(IRepository<Restaurant> restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RestaurantManagementAccessRequirement requirement, Guid restaurantId)
        {
            string? userId = context.User
                .FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<string> roles = context.User
                .FindAll(ClaimTypes.Role)
                .Select(r => r.Value);

            if(roles.Contains("Admin"))
            {
                context.Succeed(requirement);
                return;
            }

            if(roles.Contains("RestaurantOwner"))
            {
                Restaurant? restaurant = await _restaurantRepository
                    .Query()
                    .Where(r => r.Id == restaurantId)
                    .Include(r => r.Owners)
                    .FirstOrDefaultAsync();

                if (restaurant != null)
                {
                    if(restaurant.Owners.
                        Any(item => item.UserId == userId))
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
        }
    }
}
