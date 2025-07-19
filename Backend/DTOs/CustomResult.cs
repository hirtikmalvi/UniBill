using UniBill.DTOs.BusinessTypeDTOs;
using UniBill.Models;

namespace UniBill.DTOs
{
    public class CustomResult<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
        public T? Data { get; set; }
        public static CustomResult<T> Ok(T data, string message = null)
        {
            return new CustomResult<T>
            {
                Success = true,
                Message = message,
                Data = data,
            };
        }
        public static CustomResult<T> Fail(string message, List<string> errors = null)
        {
            return new CustomResult<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
}
