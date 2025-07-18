namespace UniBill.Models
{
    public class Business
    {
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string Phone { get; set; }
        public int BusinessTypeId { get; set; }
        public BusinessType BusinessType { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BusinessAddressId { get; set; }
        public BusinessAddress BusinessAddress { get; set; }
    }
}
