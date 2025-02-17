﻿using Microsoft.EntityFrameworkCore;
using StoreList.Domain.Entities;
using StoreList.Domain.Interfaces;
using StoreList.Infrastructure.Data;

namespace StoryList.Infrastructure.Repositories
{
    internal class ShoppingListRepository : IShoppingListRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShoppingList>> GetAllAsync()
        {
            return await _context.ShoppingLists
                .Include(x => x.Items)
                .ToListAsync();
        }

        public async Task<ShoppingList?> GetByIdAsync(Guid id)
        {
            return await _context.ShoppingLists
                .Include(list => list.Items)
                .FirstOrDefaultAsync(list => list.Id == id);
        }

        public async Task AddAsync(ShoppingList shoppingList)
        {
            await _context.ShoppingLists.AddAsync(shoppingList);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ShoppingList shoppingList)
        {
            var existingList = await _context.ShoppingLists
                .Include(list => list.Items)
                .FirstOrDefaultAsync(list => list.Id == shoppingList.Id);

            if (existingList == null) return;

            // Update the main shopping list properties
            existingList.Name = shoppingList.Name;

            // Update existing items and add new ones
            foreach (var item in shoppingList.Items)
            {
                var existingItem = existingList.Items.FirstOrDefault(i => i.Id == item.Id);
                if (existingItem != null)
                {
                    // Update existing item
                    existingItem.Name = item.Name;
                    existingItem.Quantity = item.Quantity;
                    existingItem.IsChecked = item.IsChecked;
                }
                else
                {
                    // Add new item
                    existingList.Items.Add(item);
                }
            }

            // Remove items that are no longer in the list
            var itemsToRemove = existingList.Items
                .Where(existingItem => !shoppingList.Items.Any(newItem => newItem.Id == existingItem.Id))
                .ToList();

            foreach (var item in itemsToRemove)
            {
                existingList.Items.Remove(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var list = await _context.ShoppingLists
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (list != null)
            {
                _context.ShoppingLists.Remove(list);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Item?> GetItemByIdAsync(Guid itemId)
        {
            return await _context.Items.FindAsync(itemId);
        }

        public async Task UpdateItemAsync(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
