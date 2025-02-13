namespace StoreList.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }

        public Guid ShoppingListId { get; set; }
        public ShoppingList? ShoppingList { get; set; }
    }
}
