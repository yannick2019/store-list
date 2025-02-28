using System.Linq;
using StoreList.Application.DTOs;
using StoreList.Application.Interfaces;
using StoreList.Domain.Entities;
using StoreList.Domain.Interfaces;

namespace StoreList.Application.Services
{
    internal class ShoppingListService : IShoppingListService
    {
        private readonly IShoppingListRepository _repository;

        public ShoppingListService(IShoppingListRepository shoppingListRepository)
        {
            _repository = shoppingListRepository;
        }

        public async Task AddAsync(ShoppingListDto shoppingListDto, string userId)
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid(),
                Name = shoppingListDto.Name,
                UserId = userId,  
                Items = shoppingListDto.Items.Select(i => new Item
                {
                    Id = Guid.NewGuid(),
                    Name = i.Name,
                    Quantity = i.Quantity,
                    IsChecked = i.IsChecked
                }).ToList()
            };
            await _repository.AddAsync(shoppingList);
        }

        public async Task<IEnumerable<ShoppingListDto>> GetAllAsync(string userId)
        {
            var shoppingLists = await _repository.GetAllAsync(userId);
            return shoppingLists.Select(list => new ShoppingListDto
            {
                Id = list.Id,
                Name = list.Name,
                UserId = list.UserId,  
                Items = list.Items.Select(item => new ItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    IsChecked = item.IsChecked
                }).ToList()
            });
        }

        public async Task<ShoppingListDto?> GetByIdAsync(Guid id, string userId)
        {
            var list = await _repository.GetByIdAsync(id, userId);
            if (list == null) return null;

            return new ShoppingListDto
            {
                Id = list.Id,
                Name = list.Name,
                UserId = list.UserId,  
                Items = list.Items.Select(item => new ItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    IsChecked = item.IsChecked
                }).ToList()
            };
        }

        public async Task UpdateAsync(ShoppingListDto shoppingListDto, string userId)
        {
            var shoppingList = new ShoppingList
            {
                Id = shoppingListDto.Id,
                Name = shoppingListDto.Name,
                UserId = userId,
                Items = shoppingListDto.Items.Select(i => new Item
                {
                    // For items without an ID, use Guid.Empty which will be detected by the repository
                    Id = i.Id == Guid.Empty ? Guid.Empty : Guid.NewGuid(),
                    Name = i.Name,
                    Quantity = i.Quantity,
                    IsChecked = i.IsChecked
                }).ToList()
            };

            await _repository.UpdateAsync(shoppingList, userId);
        }

        public async Task DeleteAsync(Guid id, string userId)
        {
            await _repository.DeleteAsync(id, userId);
        }

        public async Task<bool> UpdateItemCheckStateAsync(Guid itemId, bool isChecked, string userId)
        {
            var item = await _repository.GetItemByIdAsync(itemId, userId);
            if (item == null)
            {
                return false;
            }

            item.IsChecked = isChecked;
            await _repository.UpdateItemAsync(item, userId);
            return true;
        }
    }
}