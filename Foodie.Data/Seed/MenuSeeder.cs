using Foodie.Data.Persistance;
using Foodie.Models.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Foodie.Data.Seed
{
    /// <summary>
    /// Seeds demo menus
    /// </summary>
    public static class MenuSeeder
    {
        /// <summary>
        /// Seeds demo menus (default and lunch) for each restaurant
        /// </summary>
        /// <param name="dbContext">The application dbcontext</param>
        /// <param name="restaurants">List of restaurants</param>
        /// <returns>Seeded menus</returns>
        public static async Task<List<Menu>> SeedAsync(ApplicationDbContext dbContext, List<Restaurant> restaurants)
        {
            if (await dbContext.Menus.AnyAsync())
            {
                return await dbContext.Menus.ToListAsync();
            }

            List<Menu> menus = [];

            restaurants.Select(restaurant => restaurant.Id)
                .ToList()
                .ForEach(restaurantId =>
                {
                    menus.Add(new Menu()
                    {
                        Name = "Default menu",
                        ActiveFrom = DateTime.UtcNow,
                        ActiveTo = null,
                        RestaurantId = restaurantId,
                    });

                    menus.Add(new Menu()
                    {
                        Name = "Lunch menu",
                        ActiveFrom = DateTime.UtcNow,
                        ActiveTo = null,
                        RestaurantId = restaurantId,
                    });

                });

            await dbContext.Menus.AddRangeAsync(menus);
            await dbContext.SaveChangesAsync();
            return menus;
        }

    }
}