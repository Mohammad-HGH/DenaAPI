using DenaAPI.DTO;
using DenaAPI.Interfaces;
using DenaAPI.Responses;
using Microsoft.EntityFrameworkCore;


namespace DenaAPI.Services
{
    public class SmsService : ISmsService
    {
        private readonly DenaDbContext denaDbContext;
        public SmsService(DenaDbContext denaDbContext)
        {
            this.denaDbContext = denaDbContext;
        }


        public async Task<SmsResponse> GetSmsAsync(int smsId)
        {
            if (denaDbContext.Sms == null)
            {
                return new SmsResponse
                {
                    Error = "phone not found",
                    ErrorCode = "404",
                    Phone = string.Empty,
                    Success = false,
                };
            }
            var sms = await denaDbContext.Sms.FindAsync(smsId);

            if (sms == null)
            {
                return new SmsResponse
                {
                    Error = "phone not found",
                    ErrorCode = "404",
                    Phone = string.Empty,
                    Success = false,
                };
            }

            return new SmsResponse
            {
                Error = "ok",
                ErrorCode = "200",
                Phone = string.Empty,
                Success = true,
            };
        }
        public async Task<SmsResponse> SmsVerifyAsync(SmsRequest smsRequest)
        {
            var existingPhone = await denaDbContext.Sms.SingleOrDefaultAsync(sms => sms.SmsId == smsRequest.SmsCode && sms.UserId == smsRequest.UserId);
            if (existingPhone == null)
            {
                return new SmsResponse
                {
                    Success = false,
                    Error = "Sms code not valid!",
                    ErrorCode = "500"
                };
            }

            return new SmsResponse
            {
                Success = true,
                Message = "Login success!",
            };
        }
    }
}
