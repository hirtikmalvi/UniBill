namespace UniBill.Models
{
    public class BillItem
    {
        public int BillItemId { get; set; }
        public int BillId { get; set; }
        public Bill Bill { get; set; }
        public int ItemId  { get; set; }
        public Item Item { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal? Total { get; set; }
    }
}
