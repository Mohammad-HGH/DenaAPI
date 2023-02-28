using DenaAPI.Models;
using DenaAPI.Requests;
using DenaAPI.Responses;

namespace DenaAPI.Interfaces
{
    public interface ITokenService
    {
        Task<Tuple<string, string>> GenerateTokensAsync(int userId);
        Task<ValidateRefreshTokenResponse> ValidateRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
        Task<bool> RemoveRefreshTokenAsync(User user);
    }
}
