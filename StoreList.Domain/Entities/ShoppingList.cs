namespace StoreList.Domain.Entities
{
    public class ShoppingList
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Item> Items { get; set; } = new List<Item>();
    }
}
