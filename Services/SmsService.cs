using DenaAPI.DTO;
using DenaAPI.Interfaces;
using DenaAPI.Models;
using DenaAPI.Responses;
using Microsoft.EntityFrameworkCore;
using SmsIrRestful;

namespace DenaAPI.Services
{
    public class SmsService : ISmsService
    {
        private readonly DenaDbContext denaDbContext;
        private readonly string secretKey = "kjsfdhdsfBVJHG@#";
        private readonly string userApiKey = "4998f2cd6704ff5c5b8ce076";



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
        public async Task<SmsResponse> CreateSmsAsync(SmsRequest smsRequest)
        {
            var existingPhone = await denaDbContext.Sms.SingleOrDefaultAsync(sms => sms.Phone == smsRequest.Phone);
            Random generator = new();
            string smsCode = generator.Next(0, 1000000).ToString("D6");
            var token = new Token().GetToken(userApiKey, secretKey);
            var messageSendObject = new MessageSendObject()
            {
                Messages = new List<string> { $"بترکی!! این دفه منم با کد تایید!! {smsCode}" }.ToArray(),
                MobileNumbers = new List<string> { smsRequest.Phone }.ToArray(),
                LineNumber = "300070797197",
                SendDateTime = null,
                CanContinueInCaseOfError = true
            };
            if (existingPhone != null)
            {
                return new SmsResponse
                {
                    Success = false,
                    Error = "User already exists with the same phone",
                    ErrorCode = "S02"
                };
            }
            MessageSendResponseObject messageSendResponseObject = new MessageSend().Send(token, messageSendObject);
            if (messageSendResponseObject.IsSuccessful)
            {
                var sms = new Sms
                {
                    Phone = smsRequest.Phone,
                    SmsId = int.Parse(smsCode),
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
            else
            {
                return new SmsResponse
                {
                    Success = false,
                    Message = "Sms not sent!"
                };
            }
        }
    }
}
