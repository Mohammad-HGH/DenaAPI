namespace DenaAPI.Responses
{
    public class SmsResponse : BaseResponse
    {
        public string? Phone { get; set; }
        public string? Message { get; set; }
    }
}
