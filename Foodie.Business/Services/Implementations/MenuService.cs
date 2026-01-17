using AutoMapper;
using Foodie.Business.Repositories.Interfaces;
using Foodie.Business.Services.Interfaces;
using Foodie.Models.Domain.Entities;
using Foodie.Models.Dtos.Menus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodie.Business.Services.Implementations
{
    /// <summary>
    /// Provides menu-related operations.
    /// </summary>
    public class MenuService : IMenuService
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<Restaurant> _restaurantRepository;
        private readonly IMapper _mapper;

        public MenuService(
            IRepository<Menu> menuRepository,
            IRepository<Restaurant> restaurantRepository,
            IMapper mapper)
        {
            _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
            _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<MenuResponseDto>> GetAllAsync()
        {
            IEnumerable<Menu> menus = await _menuRepository.GetAllAsync(m => m.Restaurant);
            return _mapper.Map<IEnumerable<MenuResponseDto>>(menus);
        }

        public async Task<MenuResponseDto?> GetByIdAsync(Guid id)
        {
            Menu? menu = await _menuRepository.Query()
                .Where(m => m.Id == id)
                .Include(m => m.Restaurant)
                .Include(m => m.Categories)
                    .ThenInclude(c => c.Items)
                .FirstOrDefaultAsync();

            return _mapper.Map<MenuResponseDto?>(menu);
        }

        public async Task<IEnumerable<MenuResponseDto>> GetByRestaurantIdAsync(Guid restaurantId)
        {
            List<Menu> menus = await _menuRepository.Query()
                .Where(m => m.RestaurantId == restaurantId)
                .Include(m => m.Categories)
                    .ThenInclude(c => c.Items)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MenuResponseDto>>(menus);
        }

        public async Task<MenuResponseDto> CreateAsync(MenuCreateOrUpdateRequestDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            Restaurant? restaurant = await _restaurantRepository.GetByIdAsync(model.RestaurantId);
            if (restaurant == null)
            {
                throw new KeyNotFoundException($"Restaurant ID {model.RestaurantId} not found.");
            }

            if (model.ActiveTo.HasValue && model.ActiveTo.Value < model.ActiveFrom)
            {
                throw new ArgumentException("ActiveTo cannot be earlier than ActiveFrom.");
            }

            Menu menu = _mapper.Map<Menu>(model);
            menu.Id = Guid.NewGuid();

            await _menuRepository.AddAsync(menu);
            await _menuRepository.CommitAsync();

            MenuResponseDto response = _mapper.Map<MenuResponseDto>(menu);
            response.RestaurantName = restaurant.Name;

            return response;
        }

        public async Task<MenuResponseDto> UpdateAsync(Guid id, MenuCreateOrUpdateRequestDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            Menu? menu = await _menuRepository.GetByIdAsync(id);
            if (menu == null)
            {
                throw new KeyNotFoundException($"Menu ID {id} not found.");
            }

            Restaurant? restaurant = await _restaurantRepository.GetByIdAsync(model.RestaurantId);
            if (restaurant == null)
            {
                throw new KeyNotFoundException($"Restaurant ID {model.RestaurantId} not found.");
            }

            if (model.ActiveTo.HasValue && model.ActiveTo.Value < model.ActiveFrom)
            {
                throw new ArgumentException("ActiveTo cannot be earlier than ActiveFrom.");
            }

            _mapper.Map(model, menu);

            _menuRepository.Update(menu);
            await _menuRepository.CommitAsync();

            MenuResponseDto response = _mapper.Map<MenuResponseDto>(menu);
            response.RestaurantName = restaurant.Name;

            return response;
        }

        public async Task<MenuResponseDto> DeleteAsync(Guid id)
        {
            Menu? menu = await _menuRepository.GetByIdAsync(id);
            if (menu == null)
            {
                throw new KeyNotFoundException($"Menu ID {id} not found.");
            }

            _menuRepository.Remove(menu);
            await _menuRepository.CommitAsync();

            return _mapper.Map<MenuResponseDto>(menu);
        }
    }
}
