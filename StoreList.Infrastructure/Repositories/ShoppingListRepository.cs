using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreList.Domain.Entities;
using StoreList.Domain.Interfaces;
using StoreList.Infrastructure.Data;

namespace StoreList.Infrastructure.Repositories
{
    internal class ShoppingListRepository : IShoppingListRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShoppingList>> GetAllAsync(string userId)
        {
            return await _context.ShoppingLists
                .Include(x => x.Items)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<ShoppingList?> GetByIdAsync(Guid id, string userId)
        {
            return await _context.ShoppingLists
                .Include(list => list.Items)
                .FirstOrDefaultAsync(list => list.Id == id && list.UserId == userId);
        }

        public async Task AddAsync(ShoppingList shoppingList)
        {
            await _context.ShoppingLists.AddAsync(shoppingList);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ShoppingList shoppingList, string userId)
        {
            // First, verify the shopping list exists and belongs to the user
            var existingList = await _context.ShoppingLists
                .Include(list => list.Items)
                .FirstOrDefaultAsync(list => list.Id == shoppingList.Id && list.UserId == userId);

            if (existingList == null) return;

            // Update the basic list properties
            existingList.Name = shoppingList.Name;

            // Get all existing item IDs for comparison
            var existingItemIds = existingList.Items.Select(i => i.Id).ToHashSet();
            var newItemIds = shoppingList.Items.Where(i => i.Id != Guid.Empty).Select(i => i.Id).ToHashSet();

            // 1. Update existing items
            foreach (var newItem in shoppingList.Items.Where(i => i.Id != Guid.Empty))
            {
                var existingItem = existingList.Items.FirstOrDefault(i => i.Id == newItem.Id);
                if (existingItem != null)
                {
                    // Update properties
                    existingItem.Name = newItem.Name;
                    existingItem.Quantity = newItem.Quantity;
                    existingItem.IsChecked = newItem.IsChecked;
                }
                else
                {
                    // Skip items that don't exist - they'll be added later
                }
            }

            // 2. Remove items that are no longer present
            // Create a separate list to avoid collection modification during enumeration
            var itemsToRemove = existingList.Items
                .Where(i => !newItemIds.Contains(i.Id))
                .ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                // Two-step removal process
                existingList.Items.Remove(itemToRemove);
                _context.Items.Remove(itemToRemove);
            }

            // 3. Add new items
            // First, handle items with IDs that don't exist in the current list
            foreach (var newItem in shoppingList.Items.Where(i => i.Id != Guid.Empty && !existingItemIds.Contains(i.Id)))
            {
                var itemToAdd = new Item
                {
                    Id = newItem.Id,
                    Name = newItem.Name,
                    Quantity = newItem.Quantity,
                    IsChecked = newItem.IsChecked,
                    ShoppingListId = existingList.Id
                };
                existingList.Items.Add(itemToAdd);
            }

            // Then, handle items without IDs
            foreach (var newItem in shoppingList.Items.Where(i => i.Id == Guid.Empty))
            {
                var itemToAdd = new Item
                {
                    Id = Guid.NewGuid(),
                    Name = newItem.Name,
                    Quantity = newItem.Quantity,
                    IsChecked = newItem.IsChecked,
                    ShoppingListId = existingList.Id
                };
                existingList.Items.Add(itemToAdd);
            }

            try
            {
                // Save changes with explicit transaction
                using var transaction = await _context.Database.BeginTransactionAsync();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // If we encounter a concurrency issue
                _context.ChangeTracker.Clear(); // Clear all tracked entities

                // Start fresh with a new context operation
                var refreshedList = await _context.ShoppingLists
                    .Include(list => list.Items)
                    .FirstOrDefaultAsync(list => list.Id == shoppingList.Id && list.UserId == userId);

                if (refreshedList == null) return; // List was deleted

                // Update properties directly in the database using SQL
                refreshedList.Name = shoppingList.Name;

                // Remove all existing items and add fresh ones
                _context.Items.RemoveRange(refreshedList.Items);

                // Add all items from the update
                foreach (var item in shoppingList.Items)
                {
                    var newItem = new Item
                    {
                        Id = item.Id == Guid.Empty ? Guid.NewGuid() : item.Id,
                        Name = item.Name,
                        Quantity = item.Quantity,
                        IsChecked = item.IsChecked,
                        ShoppingListId = refreshedList.Id
                    };
                    _context.Items.Add(newItem);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id, string userId)
        {
            var list = await _context.ShoppingLists
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);

            if (list != null)
            {
                _context.ShoppingLists.Remove(list);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Item?> GetItemByIdAsync(Guid itemId, string userId)
        {
            return await _context.Items
                .Include(i => i.ShoppingList)
                .FirstOrDefaultAsync(i => i.Id == itemId && i.ShoppingList!.UserId == userId);
        }

        public async Task UpdateItemAsync(Item item, string userId)
        {
            var existingItem = await _context.Items
                .Include(i => i.ShoppingList)
                .FirstOrDefaultAsync(i => i.Id == item.Id && i.ShoppingList!.UserId == userId);

            if (existingItem != null)
            {
                existingItem.IsChecked = item.IsChecked;
                await _context.SaveChangesAsync();
            }
        }
    }
}
