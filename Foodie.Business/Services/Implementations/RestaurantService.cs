using AutoMapper;
using Foodie.Business.Repositories.Interfaces;
using Foodie.Business.Services.Interfaces;
using Foodie.Models.Domain.Entities;
using Foodie.Models.Dtos.Restaurants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodie.Business.Services.Implementations
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRepository<Restaurant> _restaurantRepository;
        private readonly IRepository<RestaurantImage> _restaurantImagesRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public RestaurantService(
            IRepository<Restaurant> restaurantRepository,
            IRepository<RestaurantImage> restaurantImagesRepository,
            IImageService imageService,
            IMapper mapper)
        {
            _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
            _restaurantImagesRepository = restaurantImagesRepository ?? throw new ArgumentNullException(nameof(restaurantImagesRepository));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<RestaurantResponseDto>> GetAllAsync()
        {
            List<Restaurant> restaurants = await _restaurantRepository.Query()
                .Include(r => r.Images)
                .Include(r => r.Owners)
                    .ThenInclude(o => o.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RestaurantResponseDto>>(restaurants);
        }

        public async Task<IEnumerable<RestaurantResponseDto>> GetByOwnerIdAsync(string ownerId)
        {
            if (string.IsNullOrWhiteSpace(ownerId))
            {
                throw new ArgumentException("Owner id cannot be empty.", nameof(ownerId));
            }

            List<Restaurant> restaurants = await _restaurantRepository.Query()
                .Where(r => r.Owners.Any(o => o.UserId == ownerId))
                .Include(r => r.Images)
                .Include(r => r.Owners)
                    .ThenInclude(o => o.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RestaurantResponseDto>>(restaurants);
        }

        public async Task<RestaurantResponseDto?> GetByIdAsync(Guid id)
        {
            Restaurant? restaurant = await _restaurantRepository.Query()
                .Where(r => r.Id == id)
                .Include(r => r.Images)
                .Include(r => r.Owners)
                    .ThenInclude(o => o.User)
                .FirstOrDefaultAsync();

            return _mapper.Map<RestaurantResponseDto?>(restaurant);
        }

        public async Task<IEnumerable<RestaurantResponseDto>> SearchByAddressAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return [];
            }

            // Note: EF Core translation depends on DB collation.
            // This stays simple and works for most setups.
            List<Restaurant> restaurants = await _restaurantRepository.Query()
                .Where(r => r.Address.Contains(address))
                .Include(r => r.Images)
                .Include(r => r.Owners)
                    .ThenInclude(o => o.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RestaurantResponseDto>>(restaurants);
        }

        public async Task<RestaurantResponseDto> CreateAsync(RestaurantCreateRequestDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            Restaurant restaurant = _mapper.Map<Restaurant>(model);
            restaurant.Id = Guid.NewGuid();

            // Add owners
            if (model.OwnerIds != null && model.OwnerIds.Count > 0)
            {
                foreach (string userId in model.OwnerIds.Distinct())
                {
                    restaurant.Owners.Add(new RestaurantOwner
                    {
                        RestaurantId = restaurant.Id,
                        UserId = userId
                    });
                }
            }

            // Process images
            if (model.Images != null && model.Images.Count > 0)
            {
                for (int i = 0; i < model.Images.Count; i++)
                {
                    string imagePath = await _imageService.UploadImageAsync(model.Images[i], "restaurants");

                    if (!string.IsNullOrWhiteSpace(imagePath))
                    {
                        restaurant.Images.Add(new RestaurantImage
                        {
                            Id = Guid.NewGuid(),
                            RestaurantId = restaurant.Id,
                            ImagePath = imagePath,
                            IsMainImage = (i == model.MainImageIndex)
                        });
                    }
                }

                EnsureSingleMainImage(restaurant);
            }

            await _restaurantRepository.AddAsync(restaurant);
            await _restaurantRepository.CommitAsync();

            Restaurant? created = await _restaurantRepository.Query()
                .Where(r => r.Id == restaurant.Id)
                .Include(r => r.Images)
                .Include(r => r.Owners)
                    .ThenInclude(o => o.User)
                .FirstOrDefaultAsync();

            return _mapper.Map<RestaurantResponseDto>(created ?? restaurant);
        }

        public async Task<RestaurantResponseDto> UpdateAsync(Guid id, RestaurantUpdateRequestDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            Restaurant? restaurant = await _restaurantRepository.Query()
                .Where(r => r.Id == id)
                .Include(r => r.Images)
                .Include(r => r.Owners)
                .FirstOrDefaultAsync();

            if (restaurant == null)
            {
                throw new KeyNotFoundException($"Restaurant ID {id} not found.");
            }

            // Update basic fields
            _mapper.Map(model, restaurant);

            // Delete selected images
            if (model.DeleteImageIds != null && model.DeleteImageIds.Count > 0)
            {
                List<RestaurantImage> toDelete = restaurant.Images
                    .Where(i => model.DeleteImageIds.Contains(i.Id))
                    .ToList();

                foreach (RestaurantImage img in toDelete)
                {
                    _imageService.DeleteImage(img.ImagePath);
                    restaurant.Images.Remove(img);
                }
            }

            // Upload new images
            if (model.NewImages != null && model.NewImages.Count > 0)
            {
                for (int i = 0; i < model.NewImages.Count; i++)
                {
                    string imagePath = await _imageService.UploadImageAsync(model.NewImages[i], "restaurants");

                    if (!string.IsNullOrWhiteSpace(imagePath))
                    {
                        restaurant.Images.Add(new RestaurantImage
                        {
                            Id = Guid.NewGuid(),
                            RestaurantId = restaurant.Id,
                            ImagePath = imagePath,
                            IsMainImage = false
                        });
                    }
                }
            }

            // Main image selection logic:
            // - If user provides MainImageIndex (index within NewImages), try to set the newly uploaded one as main
            // - Otherwise keep current main if exists, fallback to first image if none
            if (model.MainImageIndex.HasValue && model.NewImages != null && model.NewImages.Count > 0)
            {
                // Newly added images are at the end
                int newlyAddedCount = model.NewImages.Count;
                int totalCount = restaurant.Images.Count;

                int newMainIndex = totalCount - newlyAddedCount + model.MainImageIndex.Value;
                if (newMainIndex >= 0 && newMainIndex < totalCount)
                {
                    foreach (RestaurantImage img in restaurant.Images)
                    {
                        img.IsMainImage = false;
                    }

                    restaurant.Images.ElementAt(newMainIndex).IsMainImage = true;
                }
            }

            EnsureSingleMainImage(restaurant);

            // Update owners (replace set)
            if (model.OwnerIds != null)
            {
                List<string> desiredOwnerIds = model.OwnerIds
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .ToList();

                List<RestaurantOwner> ownersToRemove = restaurant.Owners
                    .Where(o => !desiredOwnerIds.Contains(o.UserId))
                    .ToList();

                foreach (RestaurantOwner owner in ownersToRemove)
                {
                    restaurant.Owners.Remove(owner);
                }

                foreach (string userId in desiredOwnerIds)
                {
                    bool exists = restaurant.Owners.Any(o => o.UserId == userId);
                    if (!exists)
                    {
                        restaurant.Owners.Add(new RestaurantOwner
                        {
                            RestaurantId = restaurant.Id,
                            UserId = userId
                        });
                    }
                }
            }

            _restaurantRepository.Update(restaurant);
            await _restaurantRepository.CommitAsync();

            Restaurant? updated = await _restaurantRepository.Query()
                .Where(r => r.Id == restaurant.Id)
                .Include(r => r.Images)
                .Include(r => r.Owners)
                    .ThenInclude(o => o.User)
                .FirstOrDefaultAsync();

            return _mapper.Map<RestaurantResponseDto>(updated ?? restaurant);
        }

        public async Task<RestaurantResponseDto> DeleteAsync(Guid id)
        {
            Restaurant? restaurant = await _restaurantRepository.Query()
                .Where(r => r.Id == id)
                .Include(r => r.Images)
                .Include(r => r.Owners)
                .FirstOrDefaultAsync();

            if (restaurant == null)
            {
                throw new KeyNotFoundException($"Restaurant ID {id} not found.");
            }

            foreach (RestaurantImage img in restaurant.Images)
            {
                _imageService.DeleteImage(img.ImagePath);
            }

            _restaurantRepository.Remove(restaurant);
            await _restaurantRepository.CommitAsync();

            return _mapper.Map<RestaurantResponseDto>(restaurant);
        }

        private static void EnsureSingleMainImage(Restaurant restaurant)
        {
            if (restaurant.Images == null || restaurant.Images.Count == 0)
            {
                return;
            }

            List<RestaurantImage> mains = restaurant.Images.Where(i => i.IsMainImage).ToList();

            if (mains.Count == 0)
            {
                restaurant.Images.ElementAt(0).IsMainImage = true;
                return;
            }

            if (mains.Count > 1)
            {
                // Keep the first main image only
                bool first = true;
                foreach (RestaurantImage img in restaurant.Images.Where(i => i.IsMainImage))
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }

                    img.IsMainImage = false;
                }
            }
        }
    }
}