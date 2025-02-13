namespace StoreList.Application.DTOs
{
    public class ShoppingListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ItemDto> Items { get; set; } = new();
    }
}
