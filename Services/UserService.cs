using Microsoft.EntityFrameworkCore;
using DenaAPI.Helpers;
using DenaAPI.Interfaces;
using DenaAPI.DTO;
using DenaAPI.Responses;
using DenaAPI.Models;


namespace DenaAPI.Services
{
    public class UserService : IUserService
    {
        private readonly DenaDbContext denaDbContext;
        private readonly ITokenService tokenService;

        public UserService(DenaDbContext denaDbContext, ITokenService tokenService)
        {
            this.denaDbContext = denaDbContext;
            this.tokenService = tokenService;
        }

        public async Task<UserResponse> GetInfoAsync(int userId)
        {
            var user = await denaDbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return new UserResponse
                {
                    Success = false,
                    Error = "No user found",
                    ErrorCode = "I001"
                };
            }

            return new UserResponse
            {
                Success = true,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreationDate = user.Ts
            };
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = denaDbContext.Users.SingleOrDefault(user => user.Active && user.Email == loginRequest.Email);

            if (user == null)
            {
                return new TokenResponse
                {
                    Success = false,
                    Error = "Email not found",
                    ErrorCode = "L02"
                };
            }
            var passwordHash = PasswordHelper.HashUsingPbkdf2(loginRequest.Password, Convert.FromBase64String(user.PasswordSalt));

            if (user.Password != passwordHash)
            {
                return new TokenResponse
                {
                    Success = false,
                    Error = "Invalid Password",
                    ErrorCode = "L03"
                };
            }

            var token = await System.Threading.Tasks.Task.Run(() => tokenService.GenerateTokensAsync(user.Id));

            return new TokenResponse
            {
                Success = true,
                AccessToken = token.Item1,
                RefreshToken = token.Item2,
                UserId = user.Id,
                FirstName = user.FirstName
            };
        }

        public async Task<LogoutResponse> LogoutAsync(int userId)
        {
            var refreshToken = await denaDbContext.RefreshTokens.FirstOrDefaultAsync(o => o.UserId == userId);

            if (refreshToken == null)
            {
                return new LogoutResponse { Success = true };
            }

            denaDbContext.RefreshTokens.Remove(refreshToken);

            var saveResponse = await denaDbContext.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new LogoutResponse { Success = true };
            }

            return new LogoutResponse { Success = false, Error = "Unable to logout user", ErrorCode = "L04" };

        }

        public async Task<SignupResponse> SignupAsync(SignupRequest signupRequest)
        {
            var existingUser = await denaDbContext.Users.SingleOrDefaultAsync(user => user.Email == signupRequest.Email);

            if (existingUser != null)
            {
                return new SignupResponse
                {
                    Success = false,
                    Error = "User already exists with the same email",
                    ErrorCode = "S02"
                };
            }

            if (signupRequest.Password != signupRequest.ConfirmPassword)
            {
                return new SignupResponse
                {
                    Success = false,
                    Error = "Password and confirm password do not match",
                    ErrorCode = "S03"
                };
            }

            if (signupRequest.Password.Length <= 7) // This can be more complicated than only length, you can check on alphanumeric and or special characters
            {
                return new SignupResponse
                {
                    Success = false,
                    Error = "Password is weak",
                    ErrorCode = "S04"
                };
            }

            var salt = PasswordHelper.GetSecureSalt();
            var passwordHash = PasswordHelper.HashUsingPbkdf2(signupRequest.Password, salt);

            var user = new User
            {
                Email = signupRequest.Email,
                Password = passwordHash,
                PasswordSalt = Convert.ToBase64String(salt),
                FirstName = signupRequest.FirstName,
                LastName = signupRequest.LastName,
                Ts = DateTime.Now,
                Active = true // You can save is false and send confirmation email to the user, then once the user confirms the email you can make it true
            };

            await denaDbContext.Users.AddAsync(user);

            var saveResponse = await denaDbContext.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new SignupResponse { Success = true, Email = user.Email };
            }

            return new SignupResponse
            {
                Success = false,
                Error = "Unable to save the user",
                ErrorCode = "S05"
            };
        }

        public async Task<UpdateResponse> UpdateAsync(UpdateRequest updateRequest)
        {

            if (updateRequest.Password != updateRequest.ConfirmPassword)
            {
                return new UpdateResponse
                {
                    Success = false,
                    Error = "Password and confirm password do not match",
                    ErrorCode = "S03"
                };
            }

            if (updateRequest.Password.Length <= 7) // This can be more complicated than only length, you can check on alphanumeric and or special characters
            {
                return new UpdateResponse
                {
                    Success = false,
                    Error = "Password is weak",
                    ErrorCode = "S04"
                };
            }
            var salt = PasswordHelper.GetSecureSalt();
            var passwordHash = PasswordHelper.HashUsingPbkdf2(updateRequest.Password, salt);

            var user = new User
            {
                Id = updateRequest.Id,
                Email = updateRequest.Email,
                Password = passwordHash,
                PasswordSalt = Convert.ToBase64String(salt),
                FirstName = updateRequest.FirstName,
                LastName = updateRequest.LastName,
                Ts = DateTime.Now,
                Active = true // You can save is false and send confirmation email to the user, then once the user confirms the email you can make it true
            };

            denaDbContext.Entry(user).State = EntityState.Modified;
            try { await denaDbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(updateRequest.Id))
                {
                    return new UpdateResponse
                    {
                        Success = false,
                        Error = "Not found",
                        ErrorCode = "S02"
                    };
                }
                else throw;
            }

            return new UpdateResponse { Success = true, Error = "User updated", ErrorCode = "S02" };
        }
        private bool UserExists(int id)
        {
            return (denaDbContext.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}