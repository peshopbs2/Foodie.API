using Foodie.Business.Authorization.Handlers;
using Foodie.Business.Authorization.Requirements;
using Foodie.Business.Repositories.Implementations;
using Foodie.Business.Repositories.Interfaces;
using Foodie.Business.Services.Implementations;
using Foodie.Business.Services.Interfaces;
using Foodie.Data.Persistance;
using Foodie.Data.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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

//JWT
var jwtKey = builder.Configuration.GetSection("Jwt").GetValue<string>("Key");
var jwtIssuer = builder.Configuration.GetSection("Jwt").GetValue<string>("Issuer");
var jwtAudience = builder.Configuration.GetSection("Jwt").GetValue<string>("Audience");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Foodie", Version = "v1" });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter ONLY the JWT token (without 'Bearer ')."
    };

    c.AddSecurityDefinition("Bearer", jwtScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IRestaurantService, RestaurantService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IMenuService, MenuService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddTransient<IJwtService, JwtService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
