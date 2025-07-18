namespace UniBill.Models
{
    public class AllowedUnit
    {
        public int BusinessTypeId { get; set; }
        public BusinessType BusinessType { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
