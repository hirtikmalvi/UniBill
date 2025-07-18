using System.ComponentModel.DataAnnotations;

namespace UniBill.DTOs.BusinessDTOs
{
    public class RegisterBusinessDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "BusinessTypeId is required.")]
        public int BusinessTypeId { get; set; }

        [Required(ErrorMessage = "Business Name is required.")]
        public string BusinessName { get; set; }

        [Required(ErrorMessage = "Phone No. is required.")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Phone No. must contain 10 digit only.")]
        public string PhoneNo {  get; set; }

        [Required(ErrorMessage = "Shop No. is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]{1,100}$", ErrorMessage = "Shop No. Can Only Contain AlphaNumeric Characters (a-zA-Z0-9).")]
        public string ShopNo { get; set; }

        [Required(ErrorMessage = "Area is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]{1,100}$", ErrorMessage = "Area Can Only Contain AlphaNumeric Characters (a-zA-Z0-9).")]
        public string Area { get; set; }


        [RegularExpression(@"^[a-zA-Z0-9\s\-]{1,100}$", ErrorMessage = "Landmark Can Only Contain AlphaNumeric Characters (a-zA-Z0-9).")]
        public string? Landmark { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\s\-]{1,100}$", ErrorMessage = "Landmark Can Only Contain AlphaNumeric Characters (a-zA-Z0-9).")]
        public string? Road { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [RegularExpression(@"^[a-zA-Z\s]{1,100}$"
        , ErrorMessage = "City Can Only Contain Alphabets (a-zA-Z).")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [RegularExpression(@"^[a-zA-Z\s]{1,100}$"
        , ErrorMessage = "State Can Only Contain Alphabets (a-zA-Z).")]
        public string State { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [RegularExpression(@"^[a-zA-Z\s]{1,100}$"
        , ErrorMessage = "Country Can Only Contain Alphabets (a-zA-Z).")]
        public string Country { get; set; }

        [Required(ErrorMessage = "PIN/Postal Code is required.")]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "PIN/Postal Code Can Only Contain 6 digits (0-9).")]
        public string PinOrPostalCode { get; set; }

    }
}
