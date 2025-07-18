namespace UniBill.Models
{
    public class AllowedItemType
    {
        public int BusinessTypeId { get; set; }
        public BusinessType BusinessType { get; set; }
        public int ItemTypeId { get; set; }
        public ItemType ItemType { get; set; }
    }
}
