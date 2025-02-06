using StoryList.Domain.Entities;

namespace StoryList.Domain.Interfaces
{
    public interface IShoppingListRepository
    {
        Task<IEnumerable<ShoppingList>> GetAllAsync();
        Task<ShoppingList?> GetByIdAsync(Guid id);
        Task AddAsync(ShoppingList shoppingList);
        Task UpdateAsync(ShoppingList shoppingList);
        Task DeleteAsync(Guid id);
    }
}
