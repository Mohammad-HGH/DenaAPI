namespace DenaAPI.DTO
{
    public class SmsRequest
    {
        public string? Phone { get; set; }
        public int VerficationId { get; set; }
        public int UserId { get; set; }
    }
}
