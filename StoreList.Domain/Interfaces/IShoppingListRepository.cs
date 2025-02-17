using StoreList.Domain.Entities;

namespace StoreList.Domain.Interfaces
{
    public interface IShoppingListRepository
    {
        Task<IEnumerable<ShoppingList>> GetAllAsync();
        Task<ShoppingList?> GetByIdAsync(Guid id);
        Task AddAsync(ShoppingList shoppingList);
        Task UpdateAsync(ShoppingList shoppingList);
        Task DeleteAsync(Guid id);
        Task<Item?> GetItemByIdAsync(Guid itemId);
        Task UpdateItemAsync(Item item);
    }
}
