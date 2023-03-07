using Microsoft.AspNetCore.Mvc;
using DenaAPI.Interfaces;
using DenaAPI.DTO;
using DenaAPI.Responses;

namespace DenaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : BaseApiController
    {
        private readonly ISmsService smsService;
        public SmsController([FromForm] ISmsService smsService) { this.smsService = smsService; }
        [HttpGet]
        [Route("GetUserSmsCode")]
        public async Task<IActionResult> GetSms(int id)
        {
            var getSmsResponse = await smsService.GetSmsAsync(id);
            if (!getSmsResponse.Success)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = "Not found",
                    ErrorCode = "S02"
                });
            }

            return Ok(getSmsResponse);
        }


        [HttpPost]
        [Route("LoginValidate")]
        public async Task<IActionResult> PostSms([FromForm] SmsRequest createSmsRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList();
                if (errors.Any())
                {
                    return BadRequest(new SmsResponse
                    {
                        Error = $"{string.Join(",", errors)}",
                        ErrorCode = "S01"
                    });
                }
            }
            var smsResponse = await smsService.SmsVerifyAsync(createSmsRequest);
            if (!smsResponse.Success)
            {
                return UnprocessableEntity(smsResponse);
            }

            return Ok(smsResponse.Phone);
        }
    }
}
