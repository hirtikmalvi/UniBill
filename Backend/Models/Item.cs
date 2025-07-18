namespace UniBill.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal ItemRate { get; set; } 
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public int ItemTypeId { get; set; }
        public ItemType ItemType { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
    }
}
