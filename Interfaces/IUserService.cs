using DenaAPI.DTO;
using DenaAPI.Models;
using DenaAPI.Responses;

namespace DenaAPI.Interfaces
{
    public interface IUserService
    {
        Task<TokenResponse> LoginAsync(LoginRequest loginRequest);
        Task<SignupResponse> SignupAsync(SignupRequest signupRequest);
        Task<LogoutResponse> LogoutAsync(int userId);
        Task<UserResponse> GetInfoAsync(int userId);
        Task<UpdateResponse> UpdateAsync(User user);
    }
}
