namespace UniBill.DTOs.BusinessAddressDTOs
{
    public class BusinessAddressDTO
    {
        public string ShopNo { get; set; }
        public string Area { get; set; }
        public string? Landmark { get; set; }
        public string? Road { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinOrPostalCode { get; set; }
    }
}
