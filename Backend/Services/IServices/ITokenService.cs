using UniBill.Models;

namespace UniBill.Services.IServices
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
