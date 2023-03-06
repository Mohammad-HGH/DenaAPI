using DenaAPI.DTO;
using DenaAPI.Models;
using DenaAPI.Responses;

namespace DenaAPI.Interfaces
{
    public interface ISmsService
    {
        Task<SmsResponse> GetSmsAsync(int smsId);
        Task<SmsResponse> SmsVerifyAsync(SmsRequest createSmsRequest);
    }
}
