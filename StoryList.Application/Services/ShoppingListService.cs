using StoryList.Application.DTOs;
using StoryList.Application.Interfaces;
using StoryList.Domain.Entities;
using StoryList.Domain.Interfaces;

namespace StoryList.Application.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly IShoppingListRepository _repository;

        public ShoppingListService(IShoppingListRepository shoppingListRepository)
        {
            _repository = shoppingListRepository;
        }

        public async Task AddAsync(ShoppingListDto shoppingListDto)
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid(),
                Name = shoppingListDto.Name,
                Items = shoppingListDto.Items.Select(i => new Item 
                {
                    Id = Guid.NewGuid(),
                    Name = i.Name,
                    Quantity = i.Quantity
                }).ToList()
            };

            await _repository.AddAsync(shoppingList);
        }

        public async Task<IEnumerable<ShoppingListDto>> GetAllAsync()
        {
            var shoppingLists = await _repository.GetAllAsync();
            return shoppingLists.Select(list => new ShoppingListDto
            {
                Id = list.Id,
                Name = list.Name,
                Items = list.Items.Select(item => new ItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity
                }).ToList()
            });
        }

        public async Task<ShoppingListDto> GetByIdAsync(Guid id)
        {
            var list = await _repository.GetByIdAsync(id);
            if (list == null) return null!;

            return new ShoppingListDto
            {
                Id = list.Id,
                Name = list.Name,
                Items = list.Items.Select(item => new ItemDto 
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

        public async Task UpdateAsync(ShoppingListDto shoppingListDto)
        {
            var shoppingList = new ShoppingList
            {
                Id = shoppingListDto.Id,
                Name = shoppingListDto.Name,
                Items = shoppingListDto.Items.Select(i => new Item 
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity
                }).ToList()
            };

            await _repository.UpdateAsync(shoppingList);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
