using Foodie.Data.Persistance;
using Foodie.Models.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Foodie.Data.Seed
{
    /// <summary>
    /// Seeds demo menu items for categories
    /// </summary>
    public static class MenuItemSeeder
    {
        private static readonly Dictionary<string, List<(string Name, string Description, decimal Price)>> _categoryData = new()
        {
            {
                "Starters",
                new()
                {
                    ("Bruschetta al Pomodoro", "Toasted homemade bread topped with fresh tomatoes, basil, and olive oil.", 8.50m),
                    ("Crispy Calamari", "Fried squid rings served with lemon and garlic sauce.", 14.90m),
                    ("Stuffed Mushrooms", "Baked mushrooms filled with cream cheese and herbs.", 10.20m),
                    ("Beef Carpaccio", "Thinly sliced raw beef fillet with parmesan and arugula.", 18.00m)
                }
            },
            {
                "Salads",
                new()
                {
                    ("Caesar Salad", "Iceberg lettuce, parmesan, croutons, and classic Caesar dressing with chicken.", 12.50m),
                    ("Greek Salad", "Tomatoes, cucumbers, red onion, feta cheese, and kalamata olives.", 11.00m),
                    ("Shopska Salad", "Traditional salad with tomatoes, cucumbers, peppers, onion, and grated white cheese.", 9.50m),
                    ("Quinoa & Avocado", "Healthy mix of quinoa, avocado, cherry tomatoes, and baby spinach.", 13.80m)
                }
            },
            {
                "Main dishes",
                new()
                {
                    ("Ribeye Steak", "Premium grilled beef steak served with roasted potatoes.", 45.00m),
                    ("Grilled Salmon", "Fresh salmon fillet with steamed vegetables and lemon butter sauce.", 32.00m),
                    ("Chicken Alfredo", "Tagliatelle pasta with creamy sauce and grilled chicken breast.", 18.50m),
                    ("BBQ Burger", "Beef patty, cheddar, bacon, onion rings, and BBQ sauce with fries.", 16.90m)
                }
            },
            {
                "Desserts",
                new()
                {
                    ("Tiramisu", "Classic Italian dessert with mascarpone and coffee-soaked ladyfingers.", 9.00m),
                    ("Chocolate Lava Cake", "Warm chocolate cake with a liquid center, served with vanilla ice cream.", 10.50m),
                    ("Cheesecake", "New York style cheesecake with strawberry topping.", 8.90m)
                }
            },
            {
                "Drinks",
                new()
                {
                    ("Mineral Water", "330ml glass bottle.", 2.50m),
                    ("Coca Cola", "330ml glass bottle.", 3.00m),
                    ("House Wine (Red)", "Glass of cabernet sauvignon.", 7.50m),
                    ("Draft Beer", "500ml lager beer.", 5.00m)
                }
            }
        };

        /// <summary>
        /// Seeds menu items based on category names
        /// </summary>
        public static async Task<List<MenuItem>> SeedAsync(ApplicationDbContext dbContext, List<MenuCategory> categories)
        {
            if (await dbContext.MenuItems.AnyAsync())
            {
                return await dbContext.MenuItems.ToListAsync();
            }

            List<MenuItem> menuItems = [];

            foreach (var category in categories)
            {
                // Проверяваме дали имаме подготвени данни за тази категория в речника
                if (_categoryData.TryGetValue(category.Name, out var itemsData))
                {
                    int displayOrder = 1;

                    foreach (var itemData in itemsData)
                    {
                        menuItems.Add(new MenuItem
                        {
                            Id = Guid.NewGuid(),
                            Name = itemData.Name,
                            Description = itemData.Description,
                            Price = itemData.Price,
                            IsActive = true,
                            IsAvailable = true,
                            DisplayOrder = displayOrder++,
                            MenuCategoryId = category.Id
                        });
                    }
                }
            }

            await dbContext.MenuItems.AddRangeAsync(menuItems);
            await dbContext.SaveChangesAsync();

            return menuItems;
        }
    }
}