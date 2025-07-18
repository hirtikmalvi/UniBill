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
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public List<BillItem> BillItems { get; set; }
    }
}
