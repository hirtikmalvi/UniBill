namespace UniBill.DTOs.BusinessDTOs
{
    public class RegisterBusinessResponseDTO
    {
        public int UserId { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public int BusinessTypeId { get; set; }
        public string AccessToken { get; set; }
    }
}
