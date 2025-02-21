using StoreList.Domain.Entities;

namespace StoreList.Domain.Interfaces
{
    public interface IShoppingListRepository
    {
        Task<IEnumerable<ShoppingList>> GetAllAsync(string userId);
        Task<ShoppingList?> GetByIdAsync(Guid id, string userId);
        Task AddAsync(ShoppingList shoppingList);
        Task UpdateAsync(ShoppingList shoppingList, string userId);
        Task DeleteAsync(Guid id, string userId);
        Task<Item?> GetItemByIdAsync(Guid itemId, string userId);
        Task UpdateItemAsync(Item item, string userId);
    }
}
