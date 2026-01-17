using Foodie.Data.Persistance;
using Foodie.Models.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Foodie.Data.Seed
{
    public static class MenuCategorySeeder
    {
        /// <summary>
        /// Seeds demo menu categories for each menu
        /// </summary>
        /// <param name="dbContext">The application dbcontext</param>
        /// <param name="menus">List of menus</param>
        /// <returns>Seeded menu categories</returns>
        public static async Task<List<MenuCategory>> SeedAsync(ApplicationDbContext dbContext, List<Menu> menus)
        {
            if (await dbContext.MenuCategories.AnyAsync())
            {
                return await dbContext.MenuCategories.ToListAsync();
            }

            List<MenuCategory> menuCategories = [];

            menus.ToList()
                .ForEach(menu =>
                {
                    List<MenuCategory> defaultMenuCategories =
                    [
                        new MenuCategory(){
                            Name = "Starters",
                            IsActive = true,
                            DisplayOrder = 0,
                            MenuId = menu.Id
                        },
                        new MenuCategory(){
                            Name = "Salads",
                            IsActive = true,
                            DisplayOrder = 1,
                            MenuId = menu.Id
                        },
                        new MenuCategory(){
                            Name = "Main dishes",
                            IsActive = true,
                            DisplayOrder = 2,
                            MenuId = menu.Id
                        },
                        new MenuCategory(){
                            Name = "Desserts",
                            IsActive = true,
                            DisplayOrder = 3,
                            MenuId = menu.Id
                        },
                        new MenuCategory(){
                            Name = "Drinks",
                            IsActive = true,
                            DisplayOrder = 4,
                            MenuId = menu.Id
                        },
                    ];

                    List<MenuCategory> lunchMenuCategories =
                    [
                        new MenuCategory(){
                            Name = "Salads",
                            IsActive = true,
                            DisplayOrder = 1,
                            MenuId = menu.Id
                        },
                        new MenuCategory(){
                            Name = "Main dishes",
                            IsActive = true,
                            DisplayOrder = 2,
                            MenuId = menu.Id
                        },
                        new MenuCategory(){
                            Name = "Desserts",
                            IsActive = true,
                            DisplayOrder = 3,
                            MenuId = menu.Id
                        }
                    ];

                    if (menu.Name == "Default menu")
                    {
                        menuCategories.AddRange(defaultMenuCategories);
                    }
                    else
                    {
                        menuCategories.AddRange(lunchMenuCategories);
                    }

                });

            await dbContext.MenuCategories.AddRangeAsync(menuCategories);
            await dbContext.SaveChangesAsync();
            return menuCategories;
        }
    }
}