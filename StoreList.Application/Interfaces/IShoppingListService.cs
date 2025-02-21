using StoreList.Application.DTOs;

namespace StoreList.Application.Interfaces
{
    public interface IShoppingListService
    {
        Task<IEnumerable<ShoppingListDto>> GetAllAsync(string userId);
        Task<ShoppingListDto?> GetByIdAsync(Guid id, string userId);
        Task AddAsync(ShoppingListDto shoppingListDto, string userId);
        Task UpdateAsync(ShoppingListDto shoppingListDto, string userId);
        Task DeleteAsync(Guid id, string userId);
        Task<bool> UpdateItemCheckStateAsync(Guid itemId, bool isChecked, string userId);
    }
}
