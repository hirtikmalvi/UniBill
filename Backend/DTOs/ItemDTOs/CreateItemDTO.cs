using System.ComponentModel.DataAnnotations;
using UniBill.Models;

namespace UniBill.DTOs.ItemDTOs
{
    public class CreateItemDTO
    {
        [Required(ErrorMessage = "ItemName is required.")]
        [Length(1, maximumLength:200, ErrorMessage = "ItemName length should be between 1-200 characters.")]
        public string ItemName { get; set; } = string.Empty;

        [Required(ErrorMessage = "ItemRate is required.")]
        [Range(0,double.MaxValue, ErrorMessage = "ItemRate must be positive.")]
        public decimal ItemRate { get; set; }

        [Required(ErrorMessage = "Unit is required.")]
        public int UnitId { get; set; }

        [Required(ErrorMessage = "ItemType is required.")]
        public int ItemTypeId { get; set; }

        //[Required(ErrorMessage = "Business is required.")]
        public int? BusinessId { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
    }
}
