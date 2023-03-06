using DenaAPI.DTO;
using DenaAPI.Interfaces;
using DenaAPI.Models;
using DenaAPI.Responses;
using Microsoft.EntityFrameworkCore;

namespace DenaAPI.Services
{
    public class SmsService : ISmsService
    {
        private readonly DenaDbContext denaDbContext;


        public SmsService(DenaDbContext denaDbContext, ITokenService tokenService)
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
        public async Task<SmsResponse> CreateSmsAsync(SmsRequest smsRequest)
        {
            var existingPhone = await denaDbContext.Sms.SingleOrDefaultAsync(sms => sms.Phone == smsRequest.Phone);

            if (existingPhone != null)
            {
                return new SmsResponse
                {
                    Success = false,
                    Error = "User already exists with the same phone",
                    ErrorCode = "S02"
                };
            }

            var sms = new Sms
            {
                Phone = smsRequest.Phone,
                SmsId = smsRequest.VerficationId,
                UserId = smsRequest.UserId,
                TS = DateTime.Now,
            };
            denaDbContext.Sms.Add(sms);

            var saveResponse = await denaDbContext.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new SmsResponse { Success = true, Phone = sms.Phone };
            }

            return new SmsResponse
            {
                Error = "ok",
                ErrorCode = "200",
                Success = true,
            };



        }
    }
}
