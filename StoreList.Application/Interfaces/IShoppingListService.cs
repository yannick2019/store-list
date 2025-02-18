﻿using StoreList.Application.DTOs;

namespace StoreList.Application.Interfaces
{
    public interface IShoppingListService
    {
        Task<IEnumerable<ShoppingListDto>> GetAllAsync();
        Task<ShoppingListDto> GetByIdAsync(Guid id);
        Task AddAsync(ShoppingListDto shoppingList);
        Task UpdateAsync(ShoppingListDto shoppingList);
        Task DeleteAsync(Guid id);
        Task<bool> UpdateItemCheckStateAsync(Guid itemId, bool isChecked);
    }
}
