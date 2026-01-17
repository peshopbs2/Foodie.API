using Foodie.Data.Persistance;
using Foodie.Models.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Data.Seed
{
    /// <summary>
    /// Seeds demo data
    /// </summary>
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            IServiceProvider scopeProvider = scope.ServiceProvider;
            ApplicationDbContext dbContext = scopeProvider.GetRequiredService<ApplicationDbContext>();

            List<Restaurant> restaurants = await RestaurantSeeder.SeedAsync(dbContext);

            List<Menu> menus = await MenuSeeder.SeedAsync(dbContext, restaurants);

            List<MenuCategory> menuCategories = await MenuCategorySeeder.SeedAsync(dbContext, menus);

            List<MenuItem> menuItems = await MenuItemSeeder.SeedAsync(dbContext, menuCategories);

            await UserSeeder.SeedAsync(serviceProvider, dbContext, restaurants);
        }
    }
}
