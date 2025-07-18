using UniBill.DTOs.BusinessAddressDTOs;
using UniBill.DTOs.BusinessTypeDTOs;
using UniBill.DTOs.UserDTOs;
using UniBill.Models;

namespace UniBill.DTOs.BusinessDTOs
{
    public class GetBusinessDTO
    {

        public int BusinessId { get; set; }
        public string BusinessName { get; set; }    
        = string.Empty;
        public string Phone {  get; set; }
        public UserDTO User { get; set; }
        public BusinessTypeDTO BusinessType { get; set; }
        public BusinessAddressDTO Address { get; set; }
    }
}
