using Foodie.Data.Persistance;
using Foodie.Models.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.Data.Seed
{
    class UserSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, ApplicationDbContext dbContext, List<Restaurant> restaurants)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            if (!userManager.Users.Any())
            {
                await SeedRoles(roleManager);
                await SeedUsers(userManager);
                await SeedRestaurantOwners(dbContext, userManager, restaurants);
            }
        }
        
        private static async Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            var adminUser = new IdentityUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true
            };
            string adminPassword = "Admin#123";
            await SeedUser(adminUser, adminPassword, "Admin", userManager);

            var ownerUser = new IdentityUser
            {
                UserName = "owner@owner.com",
                Email = "owner@owner.com",
                EmailConfirmed = true
            };
            string ownerPassword = "Owner#123";
            await SeedUser(ownerUser, ownerPassword, "RestaurantOwner", userManager);

            var ownerUser2 = new IdentityUser
            {
                UserName = "owner2@owner.com",
                Email = "owner2@owner.com",
                EmailConfirmed = true
            };
            string ownerPassword2 = "Owner#123";
            await SeedUser(ownerUser2, ownerPassword2, "RestaurantOwner", userManager);

            var user = new IdentityUser
            {
                UserName = "user@user.com",
                Email = "user@user.com",
                EmailConfirmed = true
            };
            string userPassword = "User#123";
            await SeedUser(user, userPassword, "User", userManager);
        }

        private static async Task SeedUser(IdentityUser user, string password, string roleName,
           UserManager<IdentityUser> userManager)
        {
            var userInfo = await userManager.FindByEmailAsync(user.Email);
            if (userInfo == null)
            {
                var created = await userManager
                    .CreateAsync(user, password);
                if (created.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "RestaurantOwner", "User" };
            foreach (var role in roleNames)
            {
                bool roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        // <summary>
        /// Links the demo owner user to the first few restaurants.
        /// </summary>
        private static async Task SeedRestaurantOwners(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, List<Restaurant> restaurants)
        {
            // Намираме Owner потребителя
            var ownerUser = await userManager.FindByEmailAsync("owner@owner.com");
            if (ownerUser == null || !restaurants.Any())
            {
                return;
            }

            var restaurantsToAssign = restaurants.ToList();
            bool changesMade = false;

            foreach (var restaurant in restaurantsToAssign)
            {
                // Проверяваме дали връзката вече съществува
                bool alreadyOwned = await dbContext.RestaurantOwners
                    .AnyAsync(ro => ro.RestaurantId == restaurant.Id && ro.UserId == ownerUser.Id);

                if (!alreadyOwned)
                {
                    var ownerLink = new RestaurantOwner
                    {
                        RestaurantId = restaurant.Id,
                        UserId = ownerUser.Id
                    };
                    await dbContext.RestaurantOwners.AddAsync(ownerLink);
                    changesMade = true;
                }
            }

            if (changesMade)
            {
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
