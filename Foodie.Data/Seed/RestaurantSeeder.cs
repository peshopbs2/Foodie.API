using Foodie.Data.Persistance;
using Foodie.Models.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Foodie.Data.Seed
{
    /// <summary>
    /// Seeds demo restaurants
    /// </summary>
    public static class RestaurantSeeder
    {
        /// <summary>
        /// Seeds 3 demo restaurants
        /// </summary>
        /// <param name="dbContext">The application dbcontext</param>
        /// <returns>The list of seeded restaurants</returns>
        public static async Task<List<Restaurant>> SeedAsync(ApplicationDbContext dbContext)
        {
            if(await dbContext.Restaurants.AnyAsync())
            {
                return await dbContext.Restaurants.ToListAsync();
            }

            List<Restaurant> restaurantsData = [
                new Restaurant { Id = Guid.NewGuid(), Name = "Sunset Restaurant", Address = "Slaveykov 31", Capacity = 100, Phone = "0881234567"},
                new Restaurant { Id = Guid.NewGuid(), Name = "Black Diamond", Address = "Meden Rudnik 204", Capacity = 50, Phone = "0881234568"},
                new Restaurant { Id = Guid.NewGuid(), Name = "Top Center", Address = "City Center", Capacity = 200, Phone = "0881234569"}
            ];

            await dbContext.Restaurants.AddRangeAsync(restaurantsData);
            await dbContext.SaveChangesAsync();

            var restaurants = await dbContext.Restaurants.ToListAsync();

            //seed images for each restaurant
            foreach (Restaurant restaurant in restaurants)
            {
                bool hasImages = await dbContext.RestaurantImages
                    .AnyAsync(i => i.RestaurantId == restaurant.Id);
                if(hasImages)
                {
                    continue;
                }

                List<RestaurantImage> images =
                [
                    BuildImage(restaurant, 1, isMainImage: true),
                    BuildImage(restaurant, 2, isMainImage: false),
                    BuildImage(restaurant, 3, isMainImage: false),
                ];

                await dbContext.RestaurantImages.AddRangeAsync(images);
            }
            await dbContext.SaveChangesAsync();
            return restaurants;
        }

        private static RestaurantImage BuildImage(Restaurant restaurant, int index, bool isMainImage)
        {
            string fileName = $"restaurant_{index}.jpg";
            string storagePath = $"images/restaurants/{fileName}";
            return new RestaurantImage
            {
                RestaurantId = restaurant.Id,
                ImagePath = storagePath,
                IsMainImage = isMainImage
            };
        }
    }
}