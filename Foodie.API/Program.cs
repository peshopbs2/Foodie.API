using Foodie.Business.Authorization.Handlers;
using Foodie.Business.Authorization.Requirements;
using Foodie.Business.Repositories.Implementations;
using Foodie.Business.Repositories.Interfaces;
using Foodie.Business.Services.Implementations;
using Foodie.Business.Services.Interfaces;
using Foodie.Data.Persistance;
using Foodie.Data.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//Register identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Register auth policy
builder.Services.AddScoped<IAuthorizationHandler, RestaurantManagementAccessHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManageRestaurant", policy =>
    {
        policy.Requirements.Add(new RestaurantManagementAccessRequirement());
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IRestaurantService, RestaurantService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IMenuService, MenuService>();

var app = builder.Build();
// Seed data
using IServiceScope scope = app.Services.CreateScope();
IServiceProvider serviceProvider = scope.ServiceProvider;

await DatabaseSeeder.SeedAsync(serviceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
