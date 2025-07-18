namespace UniBill.Models
{
    public class BusinessType
    {
        public int BusinessTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<AllowedItemType> AllowedItemTypes { get; set; }
        public ICollection<AllowedUnit> AllowedUnits { get; set; }
    }
}
