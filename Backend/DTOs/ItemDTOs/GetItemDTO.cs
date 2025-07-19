using UniBill.Models;

namespace UniBill.DTOs.ItemDTOs
{
    public class GetItemDTO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal ItemRate { get; set; }
        public int? CategoryId { get; set; }
        public string Category { get; set; }
        public int UnitId { get; set; }
        public string Unit { get; set; }
        public string UnitShortName { get; set; }
        public int ItemTypeId { get; set; }
        public string ItemType { get; set; }
    }
}
