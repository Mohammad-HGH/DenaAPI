using Microsoft.EntityFrameworkCore;
using DenaAPI.Helpers;
using DenaAPI.Interfaces;
using DenaAPI.DTO;
using DenaAPI.Responses;
using DenaAPI.Models;
using SmsIrRestful;

namespace DenaAPI.Services
{
    public class UserService : IUserService
    {
        private readonly DenaDbContext denaDbContext;
        private readonly ITokenService tokenService;
        private readonly string secretKey = "kjsfdhdsfBVJHG@#";
        private readonly string userApiKey = "4998f2cd6704ff5c5b8ce076";
        Random generator = new();

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
                Phone = user.Phone,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreationDate = user.Ts
            };
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = denaDbContext.Users.SingleOrDefault(user => user.Active && user.Phone == loginRequest.Phone);
            string smsCode = generator.Next(0, 1000000).ToString("D6");
            var tokenSms = new Token().GetToken(userApiKey, secretKey);
            var messageSendObject = new MessageSendObject()
            {
                Messages = new List<string> { $"بترکی!! این دفه منم با کد تایید!! {smsCode}" }.ToArray(),
                MobileNumbers = new List<string> { loginRequest.Phone }.ToArray(),
                LineNumber = "300070797197",
                SendDateTime = null,
                CanContinueInCaseOfError = true
            };




            if (user == null)
            {
                return new TokenResponse
                {
                    Success = false,
                    Error = "Phone not found",
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

            var token = await Task.Run(() => tokenService.GenerateTokensAsync(user.Id));
            MessageSendResponseObject messageSendResponseObject = new MessageSend().Send(tokenSms, messageSendObject);

            if (messageSendResponseObject.IsSuccessful)
            {
                var sms = new Sms
                {
                    Phone = loginRequest.Phone,
                    SmsId = int.Parse(smsCode),
                    UserId = user.Id,
                    TS = DateTime.Now,
                };
                denaDbContext.Sms.Add(sms);
                var saveResponse = await denaDbContext.SaveChangesAsync();
                return new TokenResponse
                {
                    Success = true,
                    AccessToken = token.Item1,
                    RefreshToken = token.Item2,
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    Message = "Sms sent"
                };
            }
            else
            {
                return new TokenResponse
                {
                    Success = false,
                    Error = "msg not sent",
                    ErrorCode = "L03"
                };
            }
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
            var existingUser = await denaDbContext.Users.SingleOrDefaultAsync(user => user.Phone == signupRequest.Phone);

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
                Phone = signupRequest.Phone,
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
                return new SignupResponse { Success = true, Phone = user.Phone };
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
                Phone = updateRequest.Phone,
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