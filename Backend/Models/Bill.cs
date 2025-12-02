namespace UniBill.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal LabourCharges { get; set; }
        public decimal FinalAmount { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public int? StatusId { get; set; }
        public BillStatus? Status { get; set; }
        public int? PaymentModeId { get; set; }
        public PaymentMode? PaymentMode { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Notes { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public List<BillItem> BillItems { get; set; }
    }
}