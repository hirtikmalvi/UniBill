namespace UniBill.Models
{
    public class AllowedCategories
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BusinessTypeId { get; set; }
        public BusinessType BusinessType { get; set; }
    }
}
