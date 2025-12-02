namespace UniBill.DTOs.BillDTOs
{
    public class UpdateBillStatusDTO
    {
        public int StatusId { get; set; }
        public int? PaymentModeId { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Notes { get; set; }
    }
}
