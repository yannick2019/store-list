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
            var existingList = await _context.ShoppingLists
                .Include(list => list.Items)
                .FirstOrDefaultAsync(list => list.Id == shoppingList.Id && list.UserId == userId);

            if (existingList == null) return;

            existingList.Name = shoppingList.Name;

            foreach (var item in shoppingList.Items)
            {
                var existingItem = existingList.Items.FirstOrDefault(i => i.Id == item.Id);
                if (existingItem != null)
                {
                    existingItem.Name = item.Name;
                    existingItem.Quantity = item.Quantity;
                    existingItem.IsChecked = item.IsChecked;
                }
                else
                {
                    item.ShoppingListId = existingList.Id;
                    existingList.Items.Add(item);
                }
            }

            var itemsToRemove = existingList.Items
                .Where(existingItem => !shoppingList.Items.Any(newItem => newItem.Id == existingItem.Id))
                .ToList();

            foreach (var item in itemsToRemove)
            {
                existingList.Items.Remove(item);
            }

            await _context.SaveChangesAsync();
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
