namespace UniBill.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ItemTypeId { get; set; }
        public ItemType ItemType { get; set; }
    }
}
