namespace UniBill.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string? Email {  get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public ICollection<Bill> Bills { get; set; }
    }
}
