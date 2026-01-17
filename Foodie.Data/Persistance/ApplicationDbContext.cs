using Foodie.Models.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Foodie.Data.Persistance;

public class ApplicationDbContext : IdentityDbContext
{

    /// <summary>
    /// Gets or sets the restaurants table database set
    /// </summary>
    public DbSet<Restaurant> Restaurants { get; set; }

    /// <summary>
    /// Gets or sets the restaurant image table database set
    /// </summary>
    public DbSet<RestaurantImage> RestaurantImages { get; set; }

    /// <summary>
    /// Gets or sets the menus table database set
    /// </summary>
    public DbSet<Menu> Menus { get; set; }

    /// <summary>
    /// Gets or sets the menu categories table database set
    /// </summary>
    public DbSet<MenuCategory> MenuCategories { get; set; }

    /// <summary>
    /// Gets or sets the menu items table database set
    /// </summary>
    public DbSet<MenuItem> MenuItems { get; set; }

    /// <summary>
    /// Gets or sets the restaurant owners table database set
    /// </summary>
    public DbSet<RestaurantOwner> RestaurantOwners { get; set; }

    /// <summary>
    /// Gets or sets the refresh tokens table database set
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Restaurant -> Images
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Images)
            .WithOne(i => i.Restaurant)
            .HasForeignKey(i => i.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restaurant -> Menus
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Menus)
            .WithOne(m => m.Restaurant)
            .HasForeignKey(m => m.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Menu -> Categories
        modelBuilder.Entity<Menu>()
            .HasMany(m => m.Categories)
            .WithOne(c => c.Menu)
            .HasForeignKey(c => c.MenuId)
            .OnDelete(DeleteBehavior.Cascade);

        // Category -> Items
        modelBuilder.Entity<MenuCategory>()
            .HasMany(c => c.Items)
            .WithOne(i => i.MenuCategory)
            .HasForeignKey(i => i.MenuCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Menu configuration
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.Property(m => m.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.HasIndex(m => new { m.RestaurantId, m.Name });

            entity.Property(m => m.ActiveFrom)
                .IsRequired();

            // ActiveTo is optional by design
        });

        // MenuCategory configuration
        modelBuilder.Entity<MenuCategory>(entity =>
        {
            entity.Property(c => c.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(c => c.DisplayOrder)
                .IsRequired();

            entity.HasIndex(c => new { c.MenuId, c.Name });
            entity.HasIndex(c => new { c.MenuId, c.DisplayOrder });
        });

        // MenuItem configuration
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.Property(i => i.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(i => i.Description)
                .HasMaxLength(2000);

            entity.Property(i => i.Price)
                .HasPrecision(18, 2);

            entity.Property(i => i.DisplayOrder)
                .IsRequired();

            entity.HasIndex(i => new { i.MenuCategoryId, i.Name });
            entity.HasIndex(i => new { i.MenuCategoryId, i.DisplayOrder });
        });

        modelBuilder.Entity<RestaurantOwner>()
        .HasKey(ro => new { ro.RestaurantId, ro.UserId });

        modelBuilder.Entity<RestaurantOwner>()
            .HasOne(ro => ro.Restaurant)
            .WithMany(r => r.Owners)
            .HasForeignKey(ro => ro.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RestaurantOwner>()
            .HasOne(ro => ro.User)
            .WithMany()
            .HasForeignKey(ro => ro.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
