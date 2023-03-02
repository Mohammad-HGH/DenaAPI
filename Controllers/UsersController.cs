using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DenaAPI.Interfaces;
using DenaAPI.Requests;
using DenaAPI.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;

namespace DenaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        private readonly IUserService userService;
        private readonly ITokenService tokenService;

        public UsersController([FromForm] IUserService userService, ITokenService tokenService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new TokenResponse
                {
                    Error = "Missing login details",
                    ErrorCode = "L01"
                });


            }

            var loginResponse = await userService.LoginAsync(loginRequest);

            if (!loginResponse.Success)
            {
                return Unauthorized(new
                {
                    loginResponse.ErrorCode,
                    loginResponse.Error
                });
            }

            return Ok(loginResponse);
        }

        [HttpPost]
        [Route("refresh_token")]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenRequest refreshTokenRequest)
        {
            if (refreshTokenRequest == null || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken) || refreshTokenRequest.UserId == 0)
            {
                return BadRequest(new TokenResponse
                {
                    Error = "Missing refresh token details",
                    ErrorCode = "R01"
                });
            }

            var validateRefreshTokenResponse = await tokenService.ValidateRefreshTokenAsync(refreshTokenRequest);

            if (!validateRefreshTokenResponse.Success)
            {
                return BadRequest(validateRefreshTokenResponse);
            }

            var tokenResponse = await tokenService.GenerateTokensAsync(validateRefreshTokenResponse.UserId);

            return Ok(new TokenResponse { AccessToken = tokenResponse.Item1, RefreshToken = tokenResponse.Item2 });
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromForm] SignupRequest signupRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList();
                if (errors.Any())
                {
                    return BadRequest(new TokenResponse
                    {
                        Error = $"{string.Join(",", errors)}",
                        ErrorCode = "S01"
                    });
                }
            }

            var signupResponse = await userService.SignupAsync(signupRequest);

            if (!signupResponse.Success)
            {
                return UnprocessableEntity(signupResponse);
            }

            return Ok(signupResponse.Email);
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var logout = await userService.LogoutAsync(UserID);

            if (!logout.Success)
            {
                return UnprocessableEntity(logout);
            }

            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> Info()
        {
            var userResponse = await userService.GetInfoAsync(UserID);

            if (!userResponse.Success)
            {
                return UnprocessableEntity(userResponse);
            }

            return Ok(userResponse);

        }

        public override bool Equals(object? obj)
        {
            return obj is UsersController controller &&
                   EqualityComparer<HttpContext>.Default.Equals(HttpContext, controller.HttpContext) &&
                   EqualityComparer<HttpRequest>.Default.Equals(Request, controller.Request) &&
                   EqualityComparer<HttpResponse>.Default.Equals(Response, controller.Response) &&
                   EqualityComparer<RouteData>.Default.Equals(RouteData, controller.RouteData) &&
                   EqualityComparer<ModelStateDictionary>.Default.Equals(ModelState, controller.ModelState) &&
                   EqualityComparer<ControllerContext>.Default.Equals(ControllerContext, controller.ControllerContext) &&
                   EqualityComparer<IModelMetadataProvider>.Default.Equals(MetadataProvider, controller.MetadataProvider) &&
                   EqualityComparer<IModelBinderFactory>.Default.Equals(ModelBinderFactory, controller.ModelBinderFactory) &&
                   EqualityComparer<IUrlHelper>.Default.Equals(Url, controller.Url) &&
                   EqualityComparer<IObjectModelValidator>.Default.Equals(ObjectValidator, controller.ObjectValidator) &&
                   EqualityComparer<ProblemDetailsFactory>.Default.Equals(ProblemDetailsFactory, controller.ProblemDetailsFactory) &&
                   EqualityComparer<ClaimsPrincipal>.Default.Equals(User, controller.User) &&
                   UserID == controller.UserID &&
                   EqualityComparer<IUserService>.Default.Equals(userService, controller.userService) &&
                   EqualityComparer<ITokenService>.Default.Equals(tokenService, controller.tokenService);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
